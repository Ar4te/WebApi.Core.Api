using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Core.Common.Global
{
    public class PageModel<T>
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int pageCount { get { return (int)Math.Ceiling((decimal)dataCount / pageSize); } }

        /// <summary>
        /// 数据总数
        /// </summary>
        public int dataCount { get; set; }

        /// <summary>
        /// 单页数量
        /// </summary>
        public int pageSize { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public List<T> data { get; set; }

        public PageModel()
        {

        }

        public PageModel(int page, int pageSize, int dataCount, List<T> data)
        {
            this.page = page;
            this.pageSize = pageSize;
            this.dataCount = dataCount;
            this.data = data;
        }

        public PageModel<TOUT> ConvertTo<TOUT>()
        {
            return new PageModel<TOUT>(page, pageSize, dataCount, default);
        }

        public PageModel<TOUT> ConvertTo<TOUT>(IMapper mapper)
        {
            var model = ConvertTo<TOUT>();

            if (data != null)
            {
                model.data = mapper.Map<List<TOUT>>(data);
            }
            return model;
        }
    }
}
