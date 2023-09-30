using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class SimpleMapper
    {
        public static T Clone<T>(T source)
        {
            return CreateTargetObject<T, T>(source);
        }

        /// <summary>
        /// Shallow Copy
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TTarget CreateTargetObject<TSource, TTarget>(TSource source)
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TTarget>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<TTarget>(source);
        }
    }
}
