using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Utilities.StateManagement
{
    /// <summary>
    /// Class used for Persisting/Retrieving Objects from state
    /// </summary>
    /// <typeparam name="T">
    /// The type of the object to persist
    /// </typeparam>
    public class StateItem<T> 
    {
        private string _name;
        private string _key;
        private T _value;
        private StateItemStore _store;
        private StatePersistenceDuration _duration = StatePersistenceDuration.Session;
        private long _ticks;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateItem&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="name">
        /// Name of the object being persisted, such as the class name.
        /// </param>
        /// <param name="key">
        /// The object key, such as the property name
        /// </param>
        /// <param name="value">
        /// The value of the object to persist.
        /// </param>
        /// <param name="store">
        /// Where the object will be persisted
        /// </param>
        /// <remarks>
        /// When the store is Cookie, the  items will be stored in a multivalue cookie named
        /// by the ObjectName property and key Object Key. When the Store is Session, the key
        /// will be ObjectName.ObjectKey
        /// NOTE: You can store items in multiple stores
        /// </remarks>
        public StateItem(string name, string key, T value, StateItemStore store)
        {
            _name = name;
            _key = key;
            _value = value;
            _store = store;
            _duration = StatePersistenceDuration.Session;
            _ticks = -1;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="StateItem&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="name">Name of the object being persisted, such as the class name.</param>
        /// <param name="key">The object key, such as the property name</param>
        /// <param name="value">The value of the object to persist.</param>
        /// <param name="store">Where the object will be persisted</param>
        /// <param name="duration">The duration.</param>
        /// <remarks>
        /// When the store is Cookie, the  items will be stored in a multivalue cookie named
        /// by the ObjectName property and key Object Key. When the Store is Session, the key
        /// will be ObjectName.ObjectKey
        /// NOTE: You can store items in multiple stores
        /// </remarks>
        public StateItem(
            string name, 
            string key, 
            T value, 
            StateItemStore store, 
            StatePersistenceDuration duration) : this(name, key, value, store)
        {
            _duration = duration;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateItem&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="name">Name of the object being persisted, such as the class name.</param>
        /// <param name="key">The object key, such as the property name</param>
        /// <param name="value">The value of the object to persist.</param>
        /// <param name="store">Where the object will be persisted</param>
        /// <param name="duration">The duration.</param>
        /// <param name="ticks">
        /// Indicates when to expire an item. 
        /// When duration is Sliding, this should be from a <see cref="System.TimeSpan"/>.
        /// When duration is Absolute, this should be from a <see cref="System.DateTime"/>
        /// </param>
        /// <remarks>
        /// When the store is Cookie, the  items will be stored in a multivalue cookie named
        /// by the ObjectName property and key Object Key. When the Store is Session, the key
        /// will be ObjectName.ObjectKey
        /// NOTE: You can store items in multiple stores
        /// </remarks>
        public StateItem(
            string name,
            string key,
            T value,
            StateItemStore store,
            StatePersistenceDuration duration,
            long ticks)
            : this(name, key, value, store, duration)
        {
            _ticks = ticks;
        }


        /// <summary>
        /// Gets or sets the name of the object to Persist. 
        /// When using the cookie store, this is the name of the cookie.
        /// When using the Session Store, the key is ObjectName.ObjectKey
        /// </summary>
        /// <value>The name of the object.</value>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the object key.
        /// When using the cookie store, this is the Cookie key.
        /// When using the Session Store, the key is ObjectName.ObjectKey
        /// </summary>
        /// <value>The object key.</value>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// Gets or sets the value to persist.
        /// </summary>
        /// <value>The value.</value>
        public T Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Where to persist the value, can be multiple stores
        /// </summary>
        /// <value>The store.</value>
        public StateItemStore Store
        {
            get { return _store; }
            set { _store = value; }
        }

        /// <summary>
        /// Gets or sets the duration to persist the object. Default is Session
        /// </summary>
        /// <value>The duration.</value>
        public StatePersistenceDuration Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        /// <summary>
        /// Gets or sets the ticks indicating when an Item should expire.
        /// When duration is Sliding, this should be from a <see cref="System.TimeSpan"/>.
        /// When duration is Absolute, this should be from a <see cref="System.DateTime"/>
        /// </summary>
        /// <value>The ticks.</value>
        public long Ticks
        {
            get { return _ticks; }
            set { _ticks = value; }
        }

    }
}
