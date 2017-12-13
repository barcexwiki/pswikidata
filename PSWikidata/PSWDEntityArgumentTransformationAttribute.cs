using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    public class PSWDEntityArgumentTransformationAttribute : ArgumentTransformationAttribute
    {
        private object ConvertArgumentElement(SessionState sessionState, object element)
        {
            if (element is string || element is PSObject && ((PSObject)element).BaseObject is string)
            {
                string qId;
                if (element is PSObject)
                {
                    qId = (string)((PSObject)element).BaseObject;
                }
                else
                {
                    qId = (string)element;
                }
                EntityProvider provider;
                PSWDSessionState state;
                PSVariable stateVariable = sessionState.PSVariable.Get("__WikidataState");

                if (stateVariable != null)
                {
                    state = (PSWDSessionState)stateVariable.Value;
                    provider = state.EntityProvider;
                }
                else
                {
                    throw new Exception("Not connected to Wikidata");
                }

                Entity entity = provider.GetEntityFromId(new EntityId(qId));
                if (entity != null)
                {
                    if (entity is Property)
                        return new PSWDProperty((Property)entity);
                    else
                    if (entity is Entity)
                        return new PSWDItem((Item)entity);
                    else
                        return element;
                }
                else
                {
                    return element;
                }
            }
            else
            {
                return element;
            }
        }

        public override object Transform(EngineIntrinsics engineIntrinsics, object inputData)
        {
            if (inputData != null)
            {
                System.Collections.ArrayList inputList;

                if (inputData is System.Collections.ICollection || inputData is PSObject && ((PSObject)inputData).BaseObject is System.Collections.ArrayList)
                {
                    if (inputData is System.Collections.ICollection)
                    {
                        inputList = new System.Collections.ArrayList((System.Collections.ICollection)inputData);
                    }
                    else
                    {
                        inputList = (System.Collections.ArrayList)((PSObject)inputData).BaseObject;
                    }

                    List<object> outputData = new List<object>();
                    foreach (object e in inputList)
                    {
                        outputData.Add(ConvertArgumentElement(engineIntrinsics.SessionState, e));
                    }
                    return outputData;
                }
                else
                {
                    return ConvertArgumentElement(engineIntrinsics.SessionState, inputData);
                }
            }
            else
            {
                return null;
            }
        }
    }
}
