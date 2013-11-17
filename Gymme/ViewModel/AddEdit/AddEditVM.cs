using System.Windows.Controls;

namespace Gymme.ViewModel.AddEdit
{
    public abstract class AddEditVM : Base.ViewModel
    {
        public UserControl Control { get; set; }

        public string PageName { get; set; }

        public string BackTarget { get; set; }

        public abstract void Rollback();

        public abstract void Commit();
    }
}
