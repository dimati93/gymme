using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Linq;

using Gymme.Data.AuxModels;

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

        public List<PersetExercise> PersetExercises { get; private set; }

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
                        XAttribute name = ex.Attribute(XName.Get("Name"));
                        if (name == null)
                        {
                            throw new NullReferenceException("Attribute 'Name' not founded");
                        }

                        return new PersetExercise { Category = categotyName, Name = name.Value };
                    });
                })
                .OrderBy(x => x.Category)
                .ThenBy(x => x.Name)
                .Select((x, i) => { x.Index = i; return x; })
                .ToList();

                IsDataLoaded = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Error in xml file parse.");
            }
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
}