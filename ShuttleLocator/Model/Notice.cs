using System.ComponentModel;
using ShuttleLocator.Annotations;

namespace ShuttleLocator.Model
{
    /// <summary>
    /// 定义远程消息数据类型
    /// </summary>
    /// <class name="Notice" interface="INotifyPropertyChanged">
    /// <attribute name="Type" type="string">消息类型："Announcement"=>通知公告;"UpdateInfo"=>更新信息</attribute>
    /// <attribute name="Title" type="string">对于通知公告：公告标题</attribute>
    /// <attribute name="Time" type="long">发布时间</attribute>
    /// <attribute name="Text" type="string">正文：公告内容或更新描述</attribute>
    /// <attribute name="Appid" type="string">为转跳到应用商店提供Appid</attribute>
    /// <method name="GetCopy" type="Notice">创建数据副本以便恢复</method>
    /// </class>
    public class Notice:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        //消息类型："Announcement"=>通知公告;"UpdateInfo"=>更新信息
        private string _type;

        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }
        //对于通知公告：标题
        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }
        //发布时间
        private string _time;

        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged("Time");
            }
        }
        //正文：公告内容或更新描述
        private string _text;

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }
        //为转跳到应用商店详细信息页面提供Appid
        private string _appid;

        public string Appid
        {
            get { return _appid; }
            set
            {
                _appid = value;
                OnPropertyChanged("Appid");
            }
        }
        //创建数据副本以便恢复
        public Notice GetCopy()
        {
            var copy = (Notice)MemberwiseClone();
            return copy;
        }
    }
}
