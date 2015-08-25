namespace MetroLogWebTarget.Core.Caching
{
    /// <summary>
    /// 缓存管理器接口
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// 获取一个项
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值</returns>
        T Get<T>(string key);


        void Set(string key, object data, int cacheTime);


        bool IsSet(string key);


        void Remove(string key);


        void RemoveByPattern(string pattern);


        void Clear();
    }
}
