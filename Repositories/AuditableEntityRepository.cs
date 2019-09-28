﻿using Penguin.Entities;
using Penguin.Messaging.Abstractions.Interfaces;
using Penguin.Messaging.Core;
using Penguin.Messaging.Persistence.Messages;
using Penguin.Persistence.Abstractions.Interfaces;
using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Penguin.Persistence.Repositories
{
    /// <summary>
    /// The base repository for entites that should have changes tracked and logged
    /// </summary>
    /// <typeparam name="T">Any type inheriting from AuditableEntity</typeparam>
    public class AuditableEntityRepository<T> : EntityRepository<T> where T : AuditableEntity, IMessageHandler<Deleting<T>>, IMessageHandler<Updating<T>>, IMessageHandler<Creating<T>>
    {
        /// <summary>
        /// An override to access all objects, does not return objects that have been deleted
        /// </summary>
        public override IQueryable<T> All => base.All.Where(e => e.DateDeleted == null);

        /// <summary>
        /// Creates a new instance of the auditable entity repository
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="messageBus"></param>
        public AuditableEntityRepository(IPersistenceContext<T> dbContext, MessageBus messageBus = null) : base(dbContext, messageBus)
        {
        }

        /// <summary>
        /// A message handler for "Created" events to set the date created
        /// </summary>
        /// <param name="createMessage">The object message containing the object</param>
        public virtual void AcceptMessage(Creating<T> createMessage)
        {
            Contract.Requires(createMessage != null);

            createMessage.Target.DateCreated = DateTime.Now;
        }

        /// <summary>
        /// A message handler for "Deleting" events to set the date deleted
        /// </summary>
        /// <param name="deleteMessage">The object message containing the object</param>
        public virtual void AcceptMessage(Deleting<T> deleteMessage)
        {
            Contract.Requires(deleteMessage != null);

            deleteMessage.Target.DateDeleted = DateTime.Now;
            deleteMessage.Target.DateModified = DateTime.Now;
        }

        /// <summary>
        /// A message handler for the "Update" event that sets the modified property
        /// </summary>
        /// <param name="updateMessage"></param>
        public virtual void AcceptMessage(Updating<T> updateMessage)
        {
            Contract.Requires(updateMessage != null);

            updateMessage.Target.DateModified = DateTime.Now;
        }
    }
}