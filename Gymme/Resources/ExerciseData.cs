using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Linq;
using Gymme.Data.AuxModels;
using Gymme.Data.Interfaces;

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
            Uri uri = new Uri("Gymme;component/Resources/ExerciseData.xml", UriKind.Relative);
            StreamResourceInfo info = Application.GetResourceStream(uri);

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

                        return new PersetExercise {Category = categotyName, Name = name.Value};
                    });
                }).Select((x, i) => { x.Index = i; return x;}).ToList();

                IsDataLoaded = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Error in xml file parse.");
            }
        }
    }
}