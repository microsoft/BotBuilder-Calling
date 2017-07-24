// 
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
// Microsoft Bot Framework: http://botframework.com
// 
// Bot Builder SDK GitHub:
// https://github.com/Microsoft/BotBuilder
// 
// Copyright (c) Microsoft Corporation
// All rights reserved.
// 
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Bot.Builder.Calling.ObjectModel.Misc;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Calling.ObjectModel.Contracts
{
    /// <summary>
    /// This is the action which customers can specify to indicate that the server call agent should place an outgoing call.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class PlaceCall : ActionBase
    {
        /// <summary>
        /// MRI for the source of the call
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Participant Source { get; set; }

        /// <summary>
        /// MRI of the user to whom the call is to be placed
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Participant Target { get; set; }

        /// <summary>
        /// Subject of the call that is to be placed
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string Subject { get; set; }

        public static readonly IEnumerable<ModalityType> DefaultModalityTypes = new ModalityType[] { ModalityType.Audio };

        private IEnumerable<ModalityType> initiateModalityTypes;

        /// <summary>
        /// The modality types the application want to present.  If none are specified,
        /// audio-only is assumed.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public IEnumerable<ModalityType> ModalityTypes
        {
            get
            {
                if (this.initiateModalityTypes == null || !this.initiateModalityTypes.Any())
                {
                    return DefaultModalityTypes;
                }
                else
                {
                    return this.initiateModalityTypes;
                }
            }

            set
            {
                this.initiateModalityTypes = value;
            }
        }
        /// <summary>
        /// AppId of the customer 
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string AppId { get; set; }

        public PlaceCall()
        {
            this.Action = ValidActions.PlaceCallAction;
        }

        public override void Validate()
        {
            base.Validate();

            Utils.AssertArgument(Target != null, "Target cannot be null");
            Utils.AssertArgument(Source != null, "Source cannot be null");
            Utils.AssertArgument(!String.IsNullOrWhiteSpace(AppId), "AppId cannot be null or empty");
            Utils.AssertArgument(this.ModalityTypes.Distinct().Count() == this.ModalityTypes.Count(), "ModalityTypes cannot contain duplicate elements.");
            Utils.AssertArgument(this.ModalityTypes.All((m) => { return m != ModalityType.Unknown; }), "ModalityTypes contains an unknown media type.");
            Utils.AssertArgument(this.ModalityTypes.All((m) => { return m != ModalityType.VideoBasedScreenSharing; }), "ModalityTypes cannot contain VideoBasedScreenSharing.");

            Source.Validate();
            Target.Validate();
            Source.Originator = true;
            Target.Originator = false;
        }
    }
}
