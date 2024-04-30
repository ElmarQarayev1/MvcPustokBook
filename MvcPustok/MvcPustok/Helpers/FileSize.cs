using System;
namespace MvcPustok.Helpers
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class FileSize:Attribute
	{
        private int _maxLength;
        public FileSize(int maxLength)
		{
            _maxLength = maxLength;
		}

        public int MaxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; }
        }
    }
}
