﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Linq;

namespace Gymme.Resources
{
    public class ExerciseData
    {
        public class Exercise
        {
            public string Name { get; set; }

            public string Category { get; set; }
        }

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

        public List<Exercise> PersetExercises { get; private set; }

        public bool IsDataLoaded { get; private set; }

        public void LoadData()
        {
            Uri uri = new Uri("/ExerciseData.xml", UriKind.Relative);
            StreamResourceInfo info = Application.GetResourceStream(uri);

            XDocument xdata = XDocument.Load(info.Stream);
            
            try
            {
                IEnumerable<XElement> categories = xdata.Root.Elements(XName.Get("Categoty"));

                PersetExercises = categories.SelectMany(x =>
                {
                    var catName = x.Attribute(XName.Get("Name"));
                    if (catName == null)
                    {
                        throw new NullReferenceException("Attribute 'Name' not founded");
                    }

                    string categotyName = catName.Value;

                    return x.Elements(XName.Get("Exercise")).Select(x2 =>
                    {
                        XAttribute name = x.Attribute(XName.Get("Name"));
                        if (name == null)
                        {
                            throw new NullReferenceException("Attribute 'Name' not founded");
                        }

                        return new Exercise {Category = categotyName, Name = name.Value};
                    });
                }).ToList();

                IsDataLoaded = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Error in xml file parse.");
            }
        }
    }
}