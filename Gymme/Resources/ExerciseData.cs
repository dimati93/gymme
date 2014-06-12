using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Linq;

using Gymme.Data.AuxModels;
using Gymme.Data.Interfaces;
using Gymme.Data.Models;
using Gymme.Data.Repository;

namespace Gymme.Resources
{
    public class ExerciseData
    {
        private static ExerciseData _instance;

        public static ExerciseData Instance
        {
            get
            {
                return _instance ?? (_instance = new ExerciseData());
            }
        }

        private ExerciseData()
        {
        }

        public List<IExercise> PersetExercises { get; private set; }

        public List<string> PersetCategories { get; private set; }

        public bool IsDataLoaded { get; private set; }

        public void LoadData()
        {
            StreamResourceInfo info = GetLocalizedResourceStream();
            XElement root = XElement.Load(info.Stream);
            
            try
            {
                IEnumerable<XElement> categories = root.Elements(XName.Get("Category"));

                PersetExercises = categories.SelectMany(x =>
                {
                    var catName = x.Attribute(XName.Get("Name"));
                    if (catName == null)
                    {
                        throw new NullReferenceException("Attribute 'Name' not founded");
                    }

                    string categotyName = catName.Value;

                    return x.Elements(XName.Get("Exercise")).Select(ex =>
                    {
                        XAttribute nameAttribute = ex.Attribute(XName.Get("Name"));
                        if (nameAttribute == null)
                        {
                            throw new NullReferenceException("Attribute 'Name' not founded");
                        }

                        XAttribute withoutWeightAttribute = ex.Attribute(XName.Get("WithoutWeight"));

                        // ReSharper disable once SimplifyConditionalTernaryExpression
                        bool withoutWeight = withoutWeightAttribute != null ? Convert.ToBoolean(withoutWeightAttribute.Value) : false;

                        return (IExercise) new PersetExercise { Category = categotyName, Name = nameAttribute.Value, WithoutWeight = withoutWeight };
                    });
                })
                .Union(RepoExercise.Instance.FindAll().Cast<IExercise>(), new ExercisePersetComparer())
                .OrderBy(x => x.Category)
                .ThenBy(x => x.Name)
                .ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Error in xml file parse.");
                IsDataLoaded = false;
                return;
            }

            PersetCategories = PersetExercises.Select(x => x.Category).Distinct().ToList();
            IsDataLoaded = false;
        }

        public void RenewPerset(Exercise item)
        {
            PersetExercises = PersetExercises.Union(new IExercise[] { item }, new ExercisePersetComparer())
                .OrderBy(x => x.Category)
                .ThenBy(x => x.Name)
                .ToList();
        }

        private StreamResourceInfo GetLocalizedResourceStream()
        {
            try
            {
                string cult = CultureInfo.CurrentUICulture.Name;
                StreamResourceInfo info = Application.GetResourceStream(GetLocalizedResourceUri(cult));
                if (info != null)
                {
                    return info;
                }
            }
            catch (Exception) {}

            return Application.GetResourceStream(GetDefaultResourceUri());
        }

        private Uri GetDefaultResourceUri()
        {
            return new Uri("Gymme;component/Resources/ExerciseData.xml", UriKind.Relative);
        }

        private Uri GetLocalizedResourceUri(string locationAbr)
        {
            return new Uri(string.Format("Gymme;component/Resources/ExerciseData.{0}.xml", locationAbr), UriKind.Relative);
        }
    }

    public class ExercisePersetComparer : IEqualityComparer<IExercise>
    {
        public bool Equals(IExercise x, IExercise y)
        {
            return StringComparer.InvariantCultureIgnoreCase.Equals(x.Name, y.Name);
        }

        public int GetHashCode(IExercise obj)
        {
            return StringComparer.InvariantCultureIgnoreCase.GetHashCode(obj.Name);
        }
    }
}