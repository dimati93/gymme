using Gymme.Resources;

namespace Gymme
{
    /// <summary>
    /// Provides access to string resources.
    /// </summary>
    public class LocalizedStrings
    {
        private static AppResources _resources = new AppResources();

        public static AppResources Resources { get { return _resources; } }
    }
}