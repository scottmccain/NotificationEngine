using System;
using System.Messaging;
using System.Reflection;
using System.Threading;
using log4net;
using NotifierService.Processors;

namespace NotifierService
{
    public class MSMQNotifierEngine
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IProcessor _notifierProcessor;
        private readonly MessageQueue _messageQueue;

        public MSMQNotifierEngine(string queueName, IProcessor notifierProcessor)
        {
            _messageQueue = new MessageQueue(queueName);
            _notifierProcessor = notifierProcessor;
            _messageQueue.Formatter = new BinaryMessageFormatter();
        }

        public void StartEngine()
        {
            IsRunning = true;

            _messageQueue.PeekCompleted += (sender, args) =>
            {
                var queue = (MessageQueue) sender;

                Message result = null;
                try
                {
                    result = queue.EndPeek(args.AsyncResult);
                }
                catch (MessageQueueException mex)
                {
                    Log.Error(mex);
                }

                if (result != null)
                {
                    ProcessBlockingTransaction(queue);
                }

                if(IsRunning)
                    queue.BeginPeek();
            };

            _messageQueue.BeginPeek();
        }

        private bool _isRunning;

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }

            set
            {
                Thread.MemoryBarrier();
                _isRunning = value;
                Thread.MemoryBarrier();
            }
        }

        private void ProcessBlockingTransaction(MessageQueue queue)
        {
            var transaction = new MessageQueueTransaction();

            var commit = false;
            try
            {
                transaction.Begin();

                var result = queue.Receive(transaction);
                if(result != null) commit = ProcessMessage(result.Body);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            finally
            {
                if(commit) transaction.Commit();
                else transaction.Abort();
            }
        }

        private bool ProcessMessage(object message)
        {
            try
            {
                return _notifierProcessor.Process(message);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return false;
        }

        //public void BlockingRecieve(TimeSpan time)
        //{
        //    var transaction = new MessageQueueTransaction();

        //    transaction.Begin();

        //    var success = false;
        //    try
        //    {
        //        var payload = GetPayload(time, transaction);
        //        if (payload == null) return;

        //        try
        //        {
        //            success = _notifierProcessor.Process(payload);
        //        }
        //        catch (Exception ex)
        //        {
        //            // LogMessage(ex);
        //        }
        //    }
        //    finally
        //    {
        //        if (success)
        //            transaction.Commit();
        //        else
        //            transaction.Abort();
        //    }
        //}

        //private object GetPayload(TimeSpan timeout, MessageQueueTransaction transaction)
        //{
        //    Message message = null;
        //    try
        //    {
        //        message = _messageQueue.Receive(timeout, transaction);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Debug(ex);
        //    }

        //    if (message == null) return null;

        //    try
        //    {
        //        return message.Body;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Debug(ex);
        //    }

        //    return null;
        //}
        public void StopEngine()
        {
            IsRunning = false;
        }
    }
}
