﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoolsLMS.DataRepos.Reposisoty
{
    public abstract class EntityBase
    {
        #region Members

        int? _requestedHashCode;

        Guid _Id;

        #endregion

        #region Properties

        /// <summary>
        /// Get or set the persisten object identifier
        /// </summary>
        [Key]
        public virtual Guid Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
            }
        }

        /// <summary>
        /// Get or set the Date of Creation 
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Get or set the Date of LastUpdate 
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime LastUpdateDate { get; set; }

        #endregion



        #region Public Methods

        /// <summary>
        /// Check if this entity is transient, ie, without identity at this moment
        /// </summary>
        /// <returns>True if entity is transient, else false</returns>
        public bool IsTransient()
        {
            return this.Id == Guid.Empty;
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// <see cref="M:System.Object.Equals"/>
        /// </summary>
        /// <param name="obj"><see cref="M:System.Object.Equals"/></param>
        /// <returns><see cref="M:System.Object.Equals"/></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is EntityBase))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            EntityBase item = (EntityBase)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return item.Id == this.Id;
        }

        /// <summary>
        /// <see cref="M:System.Object.GetHashCode"/>
        /// </summary>
        /// <returns><see cref="M:System.Object.GetHashCode"/></returns>
        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = this.Id.GetHashCode() ^ 31;

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();

        }

        #region Ctor
        /// <summary>
        /// Default Ctor
        /// </summary>
        public EntityBase()
        {
            this.CreationDate = DateTime.UtcNow;
            this.LastUpdateDate = DateTime.UtcNow;
            Id = IdentityGenerator.SequentialGuid();
        }

        #endregion

        #endregion
    }
}