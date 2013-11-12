using System.ComponentModel;
using ShuttleLocator.Annotations;

namespace ShuttleLocator.Model
{
    /// <summary>
    /// 定义站点信息数据类型
    /// </summary>
    /// <class name="Station" interface="INotifyPropertyChanged">
    /// <attribute name="Index" type="int">站点索引</attribute>
    /// <attribute name="Name" type="string">站点名称</attribute>
    /// <attribute name="Info" type="string">周边信息</attribute>
    /// <method name="GetCopy" type="Station">创建数据副本用以恢复</method>
    /// </class>
    public class Station:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        //站点索引
        private int _index;
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                OnPropertyChanged("Index");
            }
        }
        //站点名称
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        //周边信息
        private string _info;
        public string Info
        {
            get { return _info; }
            set
            {
                _info = value;
                OnPropertyChanged("Info");
            }
        }
        //创建数据副本以便恢复
        public Station GetCopy()
        {
            var copy = (Station) MemberwiseClone();
            return copy;
        }
    }
}
