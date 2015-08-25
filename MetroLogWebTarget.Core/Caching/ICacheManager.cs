namespace MetroLogWebTarget.Core.Caching
{
    /// <summary>
    /// ����������ӿ�
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// ��ȡһ����
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="key">�����</param>
        /// <returns>����ֵ</returns>
        T Get<T>(string key);


        void Set(string key, object data, int cacheTime);


        bool IsSet(string key);


        void Remove(string key);


        void RemoveByPattern(string pattern);


        void Clear();
    }
}
