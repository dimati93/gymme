using System.Windows.Input;
using System.Windows.Media;
using Gymme.Data.Models;
using Gymme.Data.Repository;
using Gymme.Resources;

namespace Gymme.ViewModel
{
    public class TrainingExerciseVM : Base.ViewModel
    {
        private readonly TrainingExercise _trainingExercise;
        private readonly Exercise _exercise;

        public TrainingExerciseVM(TrainingExercise trainingExercise)
        {
            _trainingExercise = trainingExercise;
            _exercise = RepoExercise.Instance.FindById(_trainingExercise.IdExecise);
        }

        public string Name
        {
            get
            {
                return _exercise.Name;
            }
        }

        public string Category
        {
            get
            {
                return _exercise.Category;
            }
        }
        public ICommand GotoPageViewCommand
        {
            get
            {
                return GetOrCreateCommand("GotoPageViewCommand", GotoPageView);
            }
        }

        public ICommand SkipCommand
        {
            get
            {
                return GetOrCreateCommand("SkipCommand", SkipExercise);
            }
        }

        public byte Order { get { return GetOrder(_trainingExercise.Status); }}

        public Brush StatusColor { get { return new SolidColorBrush(GetStatusColor(_trainingExercise.Status)); }}

        private byte GetOrder(TrainingExerciseStatus status)
        {
            switch (status)
            {
                case TrainingExerciseStatus.Created:
                    return 1;
                case TrainingExerciseStatus.Started:
                    return 0;
                case TrainingExerciseStatus.Skiped:
                    return 2;
                case TrainingExerciseStatus.Unfinished:
                    return 3;
                case TrainingExerciseStatus.Finished:
                    return 4;
                default:
                    return byte.MaxValue;
            }
        }

        public static Color GetStatusColor(TrainingExerciseStatus status)
        {
            switch (status)
            {
                case TrainingExerciseStatus.Created:    return AccentColors.Default;
                case TrainingExerciseStatus.Started:    return AccentColors.Started;
                case TrainingExerciseStatus.Skiped:     return AccentColors.Skiped;
                case TrainingExerciseStatus.Unfinished: return AccentColors.Missed;
                case TrainingExerciseStatus.Finished:   return AccentColors.Finished;
                default:                                return Colors.LightGray;
            }
        }

        private void GotoPageView()
        {
            _trainingExercise.Status = TrainingExerciseStatus.Started;
            _trainingExercise.Status = TrainingExerciseStatus.Finished;
            RepoTrainingExercise.Instance.Save(_trainingExercise);

            Update();

        }

        private void SkipExercise()
        {
            _trainingExercise.Status = TrainingExerciseStatus.Skiped;
            RepoTrainingExercise.Instance.Save(_trainingExercise);

            Update();
        }


        public void Update()
        {
            NotifyPropertyChanged("StatusColor");
            NotifyPropertyChanged("Order");
        }
    }
}