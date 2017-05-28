using System;

namespace DataAccess.Dal {

    /* Wrapper exception classes, so that higher layers don't have to catch
     * EF Core specific exception types -- higher layers are not supposed
     * to be coupled to a specific ORM.
     * Note we are creating custom exception only for those that the calling
     * code may need to catch and handle.
     */

    public class DataAccessException : Exception {
        public DataAccessException(string message = null,
            Exception innerException = null) : base(message, innerException) {
        }
    }

    /// <summary>
    /// An exception that is thrown when a concurrency violation is 
    /// encountered while saving to the database. A concurrency violation 
    /// occurs when an unexpected number of rows are affected during save. 
    /// This is usually because the data in the database has been modified 
    /// since it was loaded into memory.
    /// </summary>
    public class DbUpdateConcurrencyException : DataAccessException {
        private const string DefaultMessage = "An error occurred while " +
            "attempting update the database. See inner exception.";

        public DbUpdateConcurrencyException(string message = DefaultMessage,
            Exception innerException = null) : base(message, innerException) {
        }
    }

    /// <summary>
    /// An exception that is thrown when an error is encountered while saving 
    /// to the database.
    /// </summary>
    public class DbUpdateException : DataAccessException {
        private const string DefaultMessage = "An error occurred while " +
            "attempting update the database. See inner exception.";

        public DbUpdateException(string message = DefaultMessage,
            Exception innerException = null) : base(message, innerException) {
        }
    }

    ///// <summary>
    ///// The exception that is thrown when primary or foreign key is neither
    ///// default nor a valid primary key corresponding to a record in the 
    ///// database.
    ///// </summary>
    //public class InvalidKeyException : DataAccessException {
    //    public InvalidKeyException() {
    //    }
    //    public InvalidKeyException(string message) : base(message) {
    //    }
    //}

    ///// <summary>
    ///// The exception that is thrown when inserting or updating a database
    ///// record will result in a duplicate alternate key and is therefore 
    ///// not permissible.
    ///// </summary>
    //public class DuplicateAlternateKeyException : DataAccessException {
    //    public string KeyName { get; private set; }

    //    public DuplicateAlternateKeyException(string keyName) {
    //        this.KeyName = keyName;
    //    }

    //    public DuplicateAlternateKeyException(string keyName, string message)
    //        : base(message) {
    //        this.KeyName = keyName;
    //    }
    //}

    ///// <summary>
    ///// The exception that is thrown when an entity is invalid for an
    ///// operation.
    ///// </summary>
    //public class InvalidEntityException : DataAccessException {
    //    public InvalidEntityException() {
    //    }
    //    public InvalidEntityException(string message) : base(message) {
    //    }
    //}
}
