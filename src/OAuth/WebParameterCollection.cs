using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;

namespace OAuth
{
	public class WebParameterCollection : IList<WebParameter>
	{
		private IList<WebParameter> _parameters;

		public virtual WebParameter this[string name]
		{
			get
			{
				var parameters = this.Where(p => p.Name.Equals(name));

                if(parameters.Count() == 0)
				{
					return null;
				}

                if(parameters.Count() == 1)
				{
					return parameters.Single();
				}

				var value = string.Join(",", parameters.Select(p => p.Value).ToArray());
				return new WebParameter(name, value);
			}
		}

		public virtual IEnumerable<string> Names
		{
            get { return _parameters.Select(p => p.Name); }
		}

		public virtual IEnumerable<string> Values
		{
            get { return _parameters.Select(p => p.Value); }
		}

		public WebParameterCollection(IEnumerable<WebParameter> parameters)
		{
			_parameters = new List<WebParameter>(parameters.Where(IsValidParameter));
		}

		public WebParameterCollection(NameValueCollection collection)
			: this(collection.AllKeys.Select(key => new WebParameter(key, collection[key])))
		{
		}

		public WebParameterCollection(IDictionary<string, string> collection)
			: this(collection.Keys.Select(key => new WebParameter(key, collection[key])))
		{
		}

		public WebParameterCollection()
			: this(0)
		{
		}

		public WebParameterCollection(int capacity)
		{
			_parameters = new List<WebParameter>(capacity);
		}

		public void AddCollection(IDictionary<string, string> collection)
		{
			AddCollection(collection.Keys.Select(key => new WebParameter(key, collection[key])));
		}

		private void AddCollection(IEnumerable<WebParameter> collection)
		{
			foreach (var pair in collection.Where(IsValidParameter))
			{
				_parameters.Add(pair);
			}
		}

		public virtual void AddRange(NameValueCollection collection)
		{
			AddCollection(collection.AllKeys.Select(key => new WebParameter(key, collection[key])));
		}

		public virtual void AddRange(WebParameterCollection collection)
		{
			AddCollection(collection);
		}

		public virtual void AddRange(IEnumerable<WebParameter> collection)
		{
			AddCollection(collection);
		}

		public virtual void Sort(Comparison<WebParameter> comparison)
		{
			var sorted = new List<WebParameter>(_parameters);
			sorted.Sort(comparison);
			_parameters = sorted;
		}

		public virtual bool RemoveAll(IEnumerable<WebParameter> parameters)
		{
			var array = parameters.ToArray();
			var success = array.Aggregate(true, (current, parameter) => current & _parameters.Remove(parameter));
			return success && array.Length > 0;
		}

		public virtual void Add(string name, string value)
		{
			var pair = new WebParameter(name, value);
			this.Add(pair);
		}

		#region IList<WebParameter> Members

		public virtual IEnumerator<WebParameter> GetEnumerator()
		{
			return _parameters.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public virtual void Add(WebParameter parameter)
		{
			if (IsValidParameter(parameter))
			{
				_parameters.Add(parameter);
			}
		}

		public virtual void Clear()
		{
			_parameters.Clear();
		}

		public virtual bool Contains(WebParameter parameter)
		{
			return _parameters.Contains(parameter);
		}

		public virtual void CopyTo(WebParameter[] parameters, int arrayIndex)
		{
			_parameters.CopyTo(parameters, arrayIndex);
		}

		public virtual bool Remove(WebParameter parameter)
		{
			return _parameters.Remove(parameter);
		}

		public virtual int Count
		{
            get { return _parameters.Count; }
		}

		public virtual bool IsReadOnly
		{
            get { return _parameters.IsReadOnly; }
		}

		public virtual int IndexOf(WebParameter parameter)
		{
			return _parameters.IndexOf(parameter);
		}

		public virtual void Insert(int index, WebParameter parameter)
		{
			if (IsValidParameter(parameter))
			{
				_parameters.Insert(index, parameter);
			}
		}

		public virtual void RemoveAt(int index)
		{
			_parameters.RemoveAt(index);
		}

		public virtual WebParameter this[int index]
        {
            get { return _parameters[index]; }
            set { _parameters[index] = value; }
		}

		#endregion

		private static bool IsValidParameter(WebParameter parameter)
		{
			return parameter != null
				&& !String.IsNullOrEmpty(parameter.Name) && !String.IsNullOrEmpty(parameter.Name.Trim())
				&& !String.IsNullOrEmpty(parameter.Value) && !String.IsNullOrEmpty(parameter.Value.Trim());
		}
	}
}