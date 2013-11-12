using ShuttleLocator.ViewModel;

namespace ShuttleLocator.Common
{
    public class Test
    {
        public void StartTest()
        {
            var vm = new ViewModelOfStations();
            vm.GetDataModel();
        }
    }
}
