using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Domain
{
    public class PageSetting
    {
        /// <summary>
        /// 頁數
        /// </summary>
        public int PageNumber
        {
            get { return _PageNumber; }
            set { _PageNumber = value; }
        }
        /// <summary>
        /// 每筆筆數
        /// </summary>
        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }
        /// <summary>
        /// 排序的主鍵
        /// </summary>
        public String Key { get; set; }

        public int PageCount { get; set; }
        public int DataCount { get; set; }
        public Boolean TurnOnPage
        {
            get { return _TurnOnPage; }
            set { _TurnOnPage = value; }
        }

        private Boolean _TurnOnPage = false;
        private int _PageNumber = 1;
        private int _PageSize = 10;

        public PageSetting()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }

        public PageSetting(int pageNumber, int pageSize)
        {
            SetParameter(pageNumber, pageSize, "");
        }

        public void SetParameter(int pageNumber, int pageSize, String key)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Key = key;

        }


        public void SetParameter(PageSetting inputPageSeting)
        {
            this.PageNumber = inputPageSeting.PageNumber;
            this.PageSize = inputPageSeting.PageSize;
            this.Key = inputPageSeting.Key;
            TurnOnPage = true;
        }
    }
}
