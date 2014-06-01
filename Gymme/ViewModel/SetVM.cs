using Gymme.Data.Models;
using Gymme.Data.Repository;

namespace Gymme.ViewModel
{
    public class SetVM : Base.ViewModel
    {
        private readonly Set _model;
        private bool _isEdited;

        public SetVM(Set model)
        {
            _model = model;
            Lift = _model.Lift;
            Reps = _model.Reps;
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

        public float Lift { get; set; }

        public float Reps { get; set; }

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

        public void Save()
        {
            if (IsEdited)
            {
                _model.Lift = Lift;
                _model.Reps = Reps;
                RepoSet.Instance.Save(_model);
            }
        }
    }
}