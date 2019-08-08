﻿using Penguin.Entities;
using Penguin.Messaging.Core;
using Penguin.Messaging.Persistence.Messages;
using Penguin.Persistence.Abstractions.Interfaces;
using System;
using System.Linq;

namespace Penguin.Persistence.Repositories
{
    /// <summary>
    /// The base repository for entites that should have changes tracked and logged
    /// </summary>
    /// <typeparam name="T">Any type inheriting from AuditableEntity</typeparam>
    public class AuditableEntityRepository<T> : EntityRepository<T> where T : AuditableEntity
    {
        #region Properties

        /// <summary>
        /// An override to access all objects, does not return objects that have been deleted
        /// </summary>
        public override IQueryable<T> All => base.All.Where(e => e.DateDeleted == null);

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Creates a new instance of the auditable entity repository
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="messageBus"></param>
        public AuditableEntityRepository(IPersistenceContext<T> dbContext, MessageBus messageBus = null) : base(dbContext, messageBus)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// A message handler for "Created" events to set the date created
        /// </summary>
        /// <param name="create">The object message containing the object</param>
        public override void Create(Creating<T> create)
        {
            create.Target.DateCreated = DateTime.Now;

            base.Create(create);
        }

        /// <summary>
        /// An overload for the underlying "Delete" event that sets the relevant modified and deleted properties
        /// </summary>
        /// <param name="o">The objects to delete</param>
        public override void Delete(params T[] o)
        {
            foreach (T e in o)
            {
                e.DateDeleted = DateTime.Now;
                e.DateModified = DateTime.Now;
            }

            base.Delete(o);
        }

        /// <summary>
        /// A message handler for the "Update" event that sets the modified property
        /// </summary>
        /// <param name="update"></param>
        public override void Update(Updating<T> update)
        {
            update.Target.DateModified = DateTime.Now;

            base.Update(update);
        }

        #endregion Methods
    }
}