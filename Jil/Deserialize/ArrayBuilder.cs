using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Deserialize
{
    // we use ArrayBuilder rather List, as Clear on list erases all data, which is excessive
    // for our purposes. We also cache the empty array
    struct ArrayBuilder<T>
    {
        const int InitialSize = 32;

        private T[] _emptyArray;
        private T[] _data;
        private int _index;

        public void Init()
        {
            _data = new T[InitialSize];
            _index = 0;
        }

        public void Reset()
        {
            _index = 0;
        }

        public void Add(T item)
        {
            if (_index == _data.Length)
            {
                var newData = new T[_data.Length * 2];
                _data.CopyTo(newData, 0);
                _data = newData;
            }
            _data[_index] = item;
            _index++;
        }

        public T[] ToArray()
        {
            if (_index == 0)
            {
                if (_emptyArray == null)
                    _emptyArray = new T[0];
                return _emptyArray;
            }

            var result = new T[_index];
            Array.Copy(_data, 0, result, 0, _index);
            return result;
        }
    }
}
