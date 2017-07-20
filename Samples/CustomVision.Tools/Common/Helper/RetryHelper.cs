// 
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
// Microsoft Cognitive Services: https://azure.microsoft.com/en-us/services/cognitive-services
// 
// Microsoft Cognitive Services GitHub:
// https://github.com/Microsoft/Cognitive-CustomVision-Windows
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
// 

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Helper
{
    /// <summary>
    /// Helper for retry a certain function
    /// </summary>
    public static class RetryHelper
    {
        /// <summary>
        /// Retry <paramref name="funcAsync"/> for <paramref name="retryTimes"/> times at maximum.
        /// Return null, if all runs fail.
        /// </summary>
        /// <param name="funcAsync">Function to retry</param>
        /// <param name="toleratedExceptionTypes">Tolerated exceptions</param>
        /// <param name="retryTimes">Left times for retry</param>
        /// <param name="retryGapTime">Gap time between retries</param>
        /// <returns>Result of <paramref name="funcAsync"/></returns>
        public static async Task<T> RetryFuncAsync<T>(Func<Task<T>> funcAsync, ICollection<Type> toleratedExceptionTypes, int retryTimes, TimeSpan? retryGapTime = null) where T : class
        {
            try
            {
                return await funcAsync();
            }
            catch (Exception e) when (toleratedExceptionTypes.Contains(e.GetType()))
            {
                if (retryGapTime != null)
                {
                    await Task.Delay(retryGapTime.Value);
                }

                return retryTimes == 0 ? null : await RetryFuncAsync(funcAsync, toleratedExceptionTypes, retryTimes - 1, retryGapTime);
            }
        }
    }
}
