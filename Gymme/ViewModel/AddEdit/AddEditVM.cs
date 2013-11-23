using System.Windows.Controls;

namespace Gymme.ViewModel.AddEdit
{
    public abstract class AddEditVM : Base.ViewModel
    {
        protected readonly bool IsEdit;

        protected AddEditVM(bool isEdit)
        {
            BackCount = 1;
            IsEdit = isEdit;
        }

        public UserControl Control { get; set; }

        public string PageName { get; set; }

        public string BackTarget { get; set; }

        public int BackCount { get; set; }

        public abstract void Rollback();

        public abstract void Commit();
    }
}
