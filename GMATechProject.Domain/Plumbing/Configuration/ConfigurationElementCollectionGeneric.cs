namespace GMATechProject.Domain
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Collections;

    #endregion

    /// <summary>
    /// An extended implementation of the <see cref="ConfigurationElementCollection"/> which uses generics.
    /// </summary>
    /// <typeparam name="TConfigurationElement">The type of the configuration element.</typeparam>
    public class ConfigurationElementCollectionGeneric<TConfigurationElement> : ConfigurationElementCollection, IEnumerable<TConfigurationElement>
        where TConfigurationElement : ConfigurationElement, new()
    {
        #region Protected Methods

        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new TConfigurationElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">The <see cref="ConfigurationElement"/> to return the key for.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that acts as the key for the specified <see cref="ConfigurationElement"/>.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            var propertyKey = element
                // Get the type of the configuration element
                .GetType()
                // Get the proprety which has been defined as the key property for the collection
                .GetFirstPropertyWithAttribute<ConfigurationPropertyAttribute>(item => item.IsKey);

            if (propertyKey != null)
                return propertyKey.GetValue(element, null);

            throw new ConfigurationErrorsException(
                string.Format(
                    "The configuration collection {0} of {1} has no valid key.",
                    base.ElementName,
                    typeof (TConfigurationElement)));
        }

        #endregion

        #region IEnumerable<TConfigurationElement> Members

        public new IEnumerator<TConfigurationElement> GetEnumerator()
        {
            return new EnumeratorConfigurationElementCollectionGeneric(base.GetEnumerator());
        }

        #endregion

        #region Nested Type: EnumeratorConfigurationElementCollectionGeneric

        public class EnumeratorConfigurationElementCollectionGeneric : IEnumerator<TConfigurationElement>
        {
            #region Instance Variables

            private readonly IEnumerator _Enumerator;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="ConfigurationElementCollectionGeneric&lt;TConfigurationElement&gt;.EnumeratorConfigurationElementCollectionGeneric&lt;TConfigurationElement&gt;"/> class.
            /// </summary>
            /// <param name="enumerator">The enumerator.</param>
            public EnumeratorConfigurationElementCollectionGeneric(IEnumerator enumerator)
            {
                this._Enumerator = enumerator;
            }

            #endregion

            #region IEnumerator<TConfigurationElement> Members

            /// <summary>
            /// Gets the current.
            /// </summary>
            /// <value>The current.</value>
            public TConfigurationElement Current
            {
                get { return this._Enumerator.Current as TConfigurationElement; }
            }

            /// <summary>
            /// Gets the current.
            /// </summary>
            /// <value>The current.</value>
            object IEnumerator.Current
            {
                get { return this.Current; }
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                // Nothing
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
            public bool MoveNext()
            {
                return this._Enumerator.MoveNext();
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
            public void Reset()
            {
                this._Enumerator.Reset();
            }

            #endregion
        }

        #endregion
    }
}