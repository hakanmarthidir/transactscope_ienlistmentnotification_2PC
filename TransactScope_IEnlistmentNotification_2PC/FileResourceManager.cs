using System.Text;
using System.Transactions;

namespace TransactScope_IEnlistmentNotification_2PC
{
    public class FileResourceManager : IEnlistmentNotification
    {
        private string _tempFilePath;
        private bool _hasCommited = false;
        public FileResourceManager(string tempFilePath)
        {
            this._tempFilePath = tempFilePath;
            if (Transaction.Current != null)
            {
                Transaction.Current.EnlistVolatile(this, EnlistmentOptions.None);
            }            
        }

        public void Commit(Enlistment enlistment)
        {
            using (FileStream fs = File.Create(this._tempFilePath))
            {
                var data = new UTF8Encoding(true).GetBytes("Test text");
                fs.Write(data, 0, data.Length);
                _hasCommited = true;
                enlistment.Done();
            }
        }

        public void InDoubt(Enlistment enlistment)
        {
            enlistment.Done();
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            preparingEnlistment.Prepared();
        }

        public void Rollback(Enlistment enlistment)
        {
            try
            {
                if (this._hasCommited == false)
                {
                    File.Delete(this._tempFilePath);
                }
            }
            finally
            {
                enlistment.Done();
            }

        }
    }
}
