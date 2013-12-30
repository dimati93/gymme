using Gymme.Data.Models;

namespace Gymme.ViewModel
{
    public class SetVM : Base.ViewModel
    {
        private readonly Set _model;
        private bool _isEdited;

        public SetVM(Set model)
        {
            _model = model;
        }

        public Set Model
        {
            get { return _model; }
        }

        public int OrdinalNumber 
        {
            get
            {
                return _model.OrdinalNumber;
            }
        }

        public float Lift
        {
            get
            {
                return _model.Lift;
            }
            set
            {
                _model.Lift = value;
            }
        }

        public float Reps
        {
            get
            {
                return _model.Reps;
            }
            set
            {
                _model.Reps = value;
            }
        }

        public bool IsEdited
        {
            get
            {
                return _isEdited;
            }
            set
            {
                _isEdited = value;
                NotifyPropertyChanged("IsEdited");
            }
        }
    }
}