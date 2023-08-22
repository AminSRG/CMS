using AutoMapper;

namespace CustomerManagementSystem.Application
{
    public static class MapHelper
    {
        public static TDestination DynamicMap<TSource, TDestination>(TSource sourceObj) where TDestination : class
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<TDestination>(sourceObj);
        }

        public static List<TDestination> DynamicMapList<TSource, TDestination>(IEnumerable<TSource> sourceObj)
        {
            var listDes = new List<TDestination>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            });
            var mapper = config.CreateMapper();

            sourceObj.ToList().ForEach(x => listDes.Add(mapper.Map<TDestination>(x)));

            return listDes;
        }
    }
}
