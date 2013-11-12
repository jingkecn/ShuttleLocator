using System.ComponentModel;
using ShuttleLocator.Annotations;

namespace ShuttleLocator.Model
{
    /// <summary>
    /// 定义校巴状态数据类型
    /// </summary>
    /// <class name="Shuttle" interface="INotifyPropertyChanged">
    /// <attribute name="Name" type="string">校巴名称</attribute>
    /// <attribute name="Latitude" type="double">校巴所在位置的纬度</attribute>
    /// <attribute name="Longitude" type="double">校巴所在位置的经度</attribute>
    /// <attribute name="Direction" type="bool">校巴行驶的方向</attribute>
    /// <attribute name="Time" type="long">校巴处于行驶状态时本次获取数据的时间</attribute>
    /// <attribute name="Position" type="string">校巴当前位置</attribute>
    /// <attribute name="PositionIndex" type="int">校巴当前位置的索引</attribute>
    /// <attribute name="Progress" type="double">校巴的站间进度</attribute>
    /// <attribute name="IsStopping" type="bool">校巴是否停止行驶</attribute>
    /// <attribute name="IsFlying" type="bool">校巴是否不在正常路线上</attribute>
    /// <method name="GetCopy" type="Shuttle">创建数据副本以便恢复</method>
    /// </class>
    public class Shuttle:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        //校巴名称
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
        //校巴所在位置的经纬度
        private double _latitude;     //纬度
        private double _longitude;    //经度

        public double Latitude
        {
            get { return _latitude; }
            set
            {
                _latitude = value;
                OnPropertyChanged("Latitude");
            }
        }

        public double Longitude
        {
            get { return _longitude; }
            set
            {
                _longitude = value;
                OnPropertyChanged("Longitude");
            }
        }
        //校巴的行驶方向
        private bool _direction;

        public bool Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;
                OnPropertyChanged("Direction");
            }
        }
        //校巴处于行驶状态时本次获取数据的时间
        private long _time;

        public long Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged("Time");
            }
        }
        //校巴当前位置及其索引以及站间进度
        private string _position;   //当前位置
        private int _positionIndex; //站点索引
        private double _progress;   //站间进度

        public string Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged("Position");
            }
        }

        public int PositionIndex
        {
            get { return _positionIndex; }
            set
            {
                _positionIndex = value;
                OnPropertyChanged("PositionIndex");
            }
        }

        public double Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                OnPropertyChanged("Progress");
            }
        }
        //校巴是否停止行驶或者跑飞
        private bool _isStopping;   //是否停止行驶
        private bool _isFlying;     //是否跑飞

        public bool IsStopping
        {
            get { return _isStopping; }
            set
            {
                _isStopping = value;
                OnPropertyChanged("IsStopping");
            }
        }

        public bool IsFlying
        {
            get { return _isFlying; }
            set
            {
                _isFlying = value;
                OnPropertyChanged("IsFlying");
            }
        }
        //创建数据副本以便恢复
        public Shuttle GetCopy()
        {
            var copy = (Shuttle)MemberwiseClone();
            return copy;
        }
    }
}
