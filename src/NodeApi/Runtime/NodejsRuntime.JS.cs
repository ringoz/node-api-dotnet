// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.JavaScript.NodeApi.Runtime;

using System;
using System.Runtime.InteropServices;

// Imports Node.js native APIs defined in js_native_api.h
public unsafe partial class NodejsRuntime
{
#pragma warning disable IDE1006 // Naming: missing prefix '_'

    #region Misc functions

    [DllImport("__Internal")]
    private static extern napi_status napi_get_version(napi_env env, nint result_ptr);

    public override napi_status GetVersion(napi_env env, out uint result)
    {
        result = default;
        fixed (uint* result_ptr = &result)
        {
            return napi_get_version(env, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_run_script(napi_env env, napi_value script, nint result_ptr);

    public override napi_status RunScript(napi_env env, napi_value script, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_run_script(env, script, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_add_finalizer(
        napi_env env,
        napi_value value,
        nint finalizeData,
        napi_finalize finalizeCallback,
        nint finalizeHint,
        nint result_ptr);

    public override napi_status AddFinalizer(
        napi_env env,
        napi_value value,
        nint finalizeData,
        napi_finalize finalizeCallback,
        nint finalizeHint,
        out napi_ref result)
    {
        // Finalizer reference must be deleted by calling code.
        result = default;
        fixed (napi_ref* result_ptr = &result)
        {
            return napi_add_finalizer(
                env, value, finalizeData, finalizeCallback, finalizeHint, (nint)result_ptr);
        }
    }

    public override napi_status AddFinalizer(
        napi_env env,
        napi_value value,
        nint finalizeData,
        napi_finalize finalizeCallback,
        nint finalizeHint)
    {
        // Finalizer reference is deleted automatically when the GC collects the value.
        return napi_add_finalizer(
            env, value, finalizeData, finalizeCallback, finalizeHint, default);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_adjust_external_memory(
        napi_env env, long changeInBytes, nint result_ptr);

    public override napi_status AdjustExternalMemory(
        napi_env env, long changeInBytes, out long result)
    {
        result = default;
        fixed (long* result_ptr = &result)
        {
            return napi_adjust_external_memory(env, changeInBytes, (nint)result_ptr);
        }
    }

    #endregion

    #region Instance data

    [DllImport("__Internal")]
    private static extern napi_status napi_get_instance_data(napi_env env, nint result_ptr);

    public override napi_status GetInstanceData(napi_env env, out nint result)
    {
        result = default;
        fixed (nint* result_ptr = &result)
        {
            return napi_get_instance_data(env, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_set_instance_data(napi_env env,
        nint data,
        napi_finalize finalizeCallback,
        nint finalizeHint);

    public override napi_status SetInstanceData(
        napi_env env,
        nint data,
        napi_finalize finalizeCallback,
        nint finalizeHint)
    {
        return napi_set_instance_data(env, data, finalizeCallback, finalizeHint);
    }

    #endregion

    #region Error handling

    [DllImport("__Internal")]
    private static extern napi_status napi_create_error(
        napi_env env, napi_value code, napi_value msg, nint result_ptr);

    public override napi_status CreateError(
        napi_env env, napi_value code, napi_value msg, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_error(env, code, msg, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_type_error(
        napi_env env,
        napi_value code,
        napi_value msg,
        nint result_ptr);

    public override napi_status CreateTypeError(
        napi_env env,
        napi_value code,
        napi_value msg,
        out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_type_error(env, code, msg, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_range_error(
        napi_env env,
        napi_value code,
        napi_value msg,
        nint result_ptr);

    public override napi_status CreateRangeError(
        napi_env env,
        napi_value code,
        napi_value msg,
        out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_range_error(env, code, msg, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status node_api_create_syntax_error(
        napi_env env,
        napi_value code,
        napi_value msg,
        nint result_ptr);

    public override napi_status CreateSyntaxError(
        napi_env env,
        napi_value code,
        napi_value msg,
        out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return node_api_create_syntax_error(env, code, msg, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_throw(napi_env env, napi_value error);

    public override napi_status Throw(napi_env env, napi_value error)
    {
        return napi_throw(env, error);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_throw_error(napi_env env, nint code_ptr, nint msg_ptr);

    public override napi_status ThrowError(napi_env env, string? code, string msg)
    {
        using (PooledBuffer codeBuffer = PooledBuffer.FromStringUtf8(code))
        using (PooledBuffer msgBuffer = PooledBuffer.FromStringUtf8(msg))
            fixed (byte* code_ptr = codeBuffer)
            fixed (byte* msg_ptr = msgBuffer)
            {
                return napi_throw_error(
                    env,
                    (nint)code_ptr,
                    (nint)msg_ptr);
            }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_throw_type_error(napi_env env, nint code_ptr, nint msg_ptr);

    public override napi_status ThrowTypeError(napi_env env, string? code, string msg)
    {
        using (PooledBuffer codeBuffer = PooledBuffer.FromStringUtf8(code))
        using (PooledBuffer msgBuffer = PooledBuffer.FromStringUtf8(msg))
            fixed (byte* code_ptr = codeBuffer)
            fixed (byte* msg_ptr = msgBuffer)
            {
                return napi_throw_type_error(
                    env,
                    (nint)code_ptr,
                    (nint)msg_ptr);
            }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_throw_range_error(napi_env env, nint code_ptr, nint msg_ptr);

    public override napi_status ThrowRangeError(napi_env env, string? code, string msg)
    {
        using (PooledBuffer codeBuffer = PooledBuffer.FromStringUtf8(code))
        using (PooledBuffer msgBuffer = PooledBuffer.FromStringUtf8(msg))
            fixed (byte* code_ptr = codeBuffer)
            fixed (byte* msg_ptr = msgBuffer)

            {
                return napi_throw_range_error(
                    env,
                    (nint)code_ptr,
                    (nint)msg_ptr);
            }
    }

    [DllImport("__Internal")]
    private static extern napi_status node_api_throw_syntax_error(napi_env env, nint code_ptr, nint msg_ptr);

    public override napi_status ThrowSyntaxError(napi_env env, string? code, string msg)
    {
        using (PooledBuffer codeBuffer = PooledBuffer.FromStringUtf8(code))
        using (PooledBuffer msgBuffer = PooledBuffer.FromStringUtf8(msg))
            fixed (byte* code_ptr = codeBuffer)
            fixed (byte* msg_ptr = msgBuffer)
            {
                return node_api_throw_syntax_error(
                    env,
                    (nint)code_ptr,
                    (nint)msg_ptr);
            }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_is_exception_pending(napi_env env, nint result_ptr);

    public override napi_status IsExceptionPending(napi_env env, out bool result)
    {
        result = default;
        fixed (bool* result_ptr = &result)
        {
            return napi_is_exception_pending(env, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_last_error_info(napi_env env, nint result_ptr);

    public override napi_status GetLastErrorInfo(napi_env env, out napi_extended_error_info? result)
    {
        napi_extended_error_info* result_ptr;
        napi_extended_error_info** result_ptr_ptr = &result_ptr;
        napi_status status = napi_get_last_error_info(env, (nint)result_ptr_ptr);
        result = status == napi_status.napi_ok ? *result_ptr : null;
        return status;
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_and_clear_last_exception(napi_env env, nint result_ptr);

    public override napi_status GetAndClearLastException(napi_env env, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_get_and_clear_last_exception(env, (nint)result_ptr);
        }
    }

    #endregion

    #region Value type checking

    [DllImport("__Internal")]
    private static extern napi_status napi_typeof(
        napi_env env,
        napi_value value,
        nint result_ptr);

    public override napi_status GetValueType(
        napi_env env,
        napi_value value,
        out napi_valuetype result)
    {
        result = default;
        fixed (napi_valuetype* result_ptr = &result)
        {
            return napi_typeof(env, value, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_is_date(
        napi_env env,
        napi_value value,
        nint result_ptr);

    public override napi_status IsDate(napi_env env, napi_value value, out bool result)
    {
        c_bool resultBool = default;
        c_bool* result_ptr = &resultBool;
        napi_status status = napi_is_date(env, value, (nint)result_ptr);
        result = (bool)resultBool;
        return status;
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_is_promise(
        napi_env env,
        napi_value value,
        nint result_ptr);

    public override napi_status IsPromise(napi_env env, napi_value value, out bool result)
    {
        c_bool resultBool = default;
        c_bool* result_ptr = &resultBool;
        napi_status status = napi_is_promise(env, value, (nint)result_ptr);
        result = (bool)resultBool;
        return status;
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_is_error(
        napi_env env,
        napi_value value,
        nint result_ptr);

    public override napi_status IsError(napi_env env, napi_value value, out bool result)
    {
        c_bool resultBool = default;
        c_bool* result_ptr = &resultBool;
        napi_status status = napi_is_error(env, value, (nint)result_ptr);
        result = (bool)resultBool;
        return status;
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_is_array(
        napi_env env,
        napi_value value,
        nint result_ptr);

    public override napi_status IsArray(napi_env env, napi_value value, out bool result)
    {
        c_bool resultBool = default;
        c_bool* result_ptr = &resultBool;
        napi_status status = napi_is_array(env, value, (nint)result_ptr);
        result = (bool)resultBool;
        return status;
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_is_arraybuffer(
        napi_env env,
        napi_value value,
        nint result_ptr);

    public override napi_status IsArrayBuffer(napi_env env, napi_value value, out bool result)
    {
        c_bool resultBool = default;
        c_bool* result_ptr = &resultBool;
        napi_status status = napi_is_arraybuffer(env, value, (nint)result_ptr);
        result = (bool)resultBool;
        return status;
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_is_detached_arraybuffer(
        napi_env env,
        napi_value value,
        nint result_ptr);

    public override napi_status IsDetachedArrayBuffer(
        napi_env env,
        napi_value value,
        out bool result)
    {
        c_bool resultBool = default;
        c_bool* result_ptr = &resultBool;
        napi_status status = napi_is_detached_arraybuffer(env, value, (nint)result_ptr);
        result = (bool)resultBool;
        return status;
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_is_typedarray(
        napi_env env,
        napi_value value,
        nint result_ptr);

    public override napi_status IsTypedArray(napi_env env, napi_value value, out bool result)
    {
        c_bool resultBool = default;
        c_bool* result_ptr = &resultBool;
        napi_status status = napi_is_typedarray(env, value, (nint)result_ptr);
        result = (bool)resultBool;
        return status;
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_is_dataview(
        napi_env env,
        napi_value value,
        nint result_ptr);

    public override napi_status IsDataView(napi_env env, napi_value value, out bool result)
    {
        c_bool resultBool = default;
        c_bool* result_ptr = &resultBool;
        napi_status status = napi_is_dataview(env, value, (nint)result_ptr);
        result = (bool)resultBool;
        return status;
    }

    #endregion

    #region Value retrieval

    [DllImport("__Internal")]
    private static extern napi_status napi_get_value_double(
        napi_env env,
        napi_value value,
        nint result_ptr);

    public override napi_status GetValueDouble(napi_env env, napi_value value, out double result)
    {
        result = default;
        fixed (double* result_ptr = &result)
        {
            return napi_get_value_double(env, value, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_value_int32(
        napi_env env,
        napi_value value,
        nint result_ptr);

    public override napi_status GetValueInt32(napi_env env, napi_value value, out int result)
    {
        result = default;
        fixed (int* result_ptr = &result)
        {
            return napi_get_value_int32(env, value, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_value_uint32(
        napi_env env,
        napi_value value,
        nint result_ptr);

    public override napi_status GetValueUInt32(napi_env env, napi_value value, out uint result)
    {
        result = default;
        fixed (uint* result_ptr = &result)
        {
            return napi_get_value_uint32(env, value, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_value_int64(
        napi_env env,
        napi_value value,
        nint result_ptr);

    public override napi_status GetValueInt64(napi_env env, napi_value value, out long result)
    {
        result = default;
        fixed (long* result_ptr = &result)
        {
            return napi_get_value_int64(env, value, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_value_bigint_int64(
        napi_env env,
        napi_value value,
        nint result_ptr,
        nint lossless_ptr);

    public override napi_status GetValueBigInt64(
        napi_env env, napi_value value, out long result, out bool lossless)
    {
        result = default;
        lossless = default;
        fixed (long* result_ptr = &result)
        fixed (bool* lossless_ptr = &lossless)
        {
            return napi_get_value_bigint_int64(
                env, value, (nint)result_ptr, (nint)lossless_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_value_bigint_uint64(
        napi_env env,
        napi_value value,
        nint result_ptr,
        nint lossless_ptr);

    public override napi_status GetValueBigInt64(
        napi_env env, napi_value value, out ulong result, out bool lossless)
    {
        result = default;
        lossless = default;
        fixed (ulong* result_ptr = &result)
        fixed (bool* lossless_ptr = &lossless)
        {
            return napi_get_value_bigint_uint64(
                env, value, (nint)result_ptr, (nint)lossless_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_value_bigint_words(
        napi_env env,
        napi_value value,
        nint sign_ptr,
        nint result_ptr,
        nint words_ptr);

    public override napi_status GetBigIntWordCount(napi_env env, napi_value value, out nuint result)
    {
        result = 0;
        fixed (nuint* result_ptr = &result)
        {
            // sign and words pointers must be null when we just want to get the length.
            return napi_get_value_bigint_words(
                env, value, default, (nint)result_ptr, default);
        }
    }

    public override napi_status GetBigIntWords(
        napi_env env, napi_value value, out int sign, Span<ulong> words, out nuint result)
    {
        sign = default;
        result = (nuint)words.Length;
        fixed (int* sign_ptr = &sign)
        fixed (ulong* words_ptr = words)
        fixed (nuint* result_ptr = &result)
        {
            return napi_get_value_bigint_words(
                env, value, (nint)sign_ptr, (nint)result_ptr, (nint)words_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_value_bool(
        napi_env env,
        napi_value value,
        nint result_ptr);

    public override napi_status GetValueBool(napi_env env, napi_value value, out bool result)
    {
        result = default;
        fixed (bool* result_ptr = &result)
        {
            return napi_get_value_bool(env, value, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_value_string_utf8(
        napi_env env,
        napi_value value,
        nint buf_ptr,
        nuint bufsize,
        nint result_ptr);

    public override napi_status GetValueStringUtf8(
        napi_env env, napi_value value, Span<byte> buf, out nuint result)
    {
        fixed (nuint* result_ptr = &result)
        fixed (byte* buf_ptr = &buf.GetPinnableReference())
        {
            return napi_get_value_string_utf8(
                env, value, (nint)buf_ptr, (nuint)buf.Length, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_value_string_utf16(
        napi_env env,
        napi_value value,
        nint buf_ptr,
        nuint bufsize,
        nint result_ptr);

    public override napi_status GetValueStringUtf16(
        napi_env env, napi_value value, Span<char> buf, out nuint result)
    {
        fixed (nuint* result_ptr = &result)
        fixed (char* buf_ptr = &buf.GetPinnableReference())
        {
            return napi_get_value_string_utf16(
                env, value, (nint)buf_ptr, (nuint)buf.Length, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_date_value(
        napi_env env,
        napi_value value,
        nint result_ptr);

    public override napi_status GetValueDate(napi_env env, napi_value value, out double result)
    {
        result = default;
        fixed (double* result_ptr = &result)
        {
            return napi_get_date_value(env, value, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status node_api_symbol_for(
        napi_env env,
        nint buf_ptr,
        nuint bufsize,
        nint result_ptr);

    public override napi_status GetSymbolFor(napi_env env, string name, out napi_value result)
    {
        result = default;
        using (PooledBuffer nameBuffer = PooledBuffer.FromStringUtf8(name))
            fixed (byte* name_ptr = nameBuffer)
            fixed (napi_value* result_ptr = &result)
            {
                return node_api_symbol_for(
                    env,
                    (nint)name_ptr,
                    (nuint)nameBuffer.Length,
                    (nint)result_ptr);
            }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_array_length(
        napi_env env,
        napi_value value,
        nint result_ptr);

    public override napi_status GetArrayLength(napi_env env, napi_value value, out uint result)
    {
        result = default;
        fixed (uint* result_ptr = &result)
        {
            return napi_get_array_length(env, value, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_arraybuffer_info(
        napi_env env,
        napi_value value,
        nint data_ptr,
        nint length_ptr);

    public override napi_status GetArrayBufferInfo(
        napi_env env, napi_value value, out nint data, out nuint length)
    {
        data = default;
        length = default;
        fixed (nint* data_ptr = &data)
        fixed (nuint* length_ptr = &length)
        {
            return napi_get_arraybuffer_info(
                env, value, (nint)data_ptr, (nint)length_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_dataview_info(
        napi_env env,
        napi_value value,
        nint length_ptr,
        nint data_ptr,
        nint arraybuffer_ptr,
        nint offset_ptr);

    public override napi_status GetDataViewInfo(
        napi_env env,
        napi_value value,
        out nuint byteLength,
        out nint data,
        out napi_value arraybuffer,
        out nuint offset)
    {
        byteLength = default;
        data = default;
        arraybuffer = default;
        offset = default;
        fixed (nuint* length_ptr = &byteLength)
        fixed (nint* data_ptr = &data)
        fixed (napi_value* arraybuffer_ptr = &arraybuffer)
        fixed (nuint* offset_ptr = &offset)
        {
            return napi_get_dataview_info(
                env,
                value,
                (nint)length_ptr,
                (nint)data_ptr,
                (nint)arraybuffer_ptr,
                (nint)offset_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_typedarray_info(
        napi_env env,
        napi_value value,
        nint type_ptr,
        nint length_ptr,
        nint data_ptr,
        nint arraybuffer_ptr,
        nint offset_ptr);

    public override napi_status GetTypedArrayInfo(
        napi_env env,
        napi_value value,
        out napi_typedarray_type type,
        out nuint byteLength,
        out nint data,
        out napi_value arraybuffer,
        out nuint offset)
    {
        type = default;
        byteLength = default;
        data = default;
        arraybuffer = default;
        offset = default;
        fixed (napi_typedarray_type* type_ptr = &type)
        fixed (nuint* length_ptr = &byteLength)
        fixed (nint* data_ptr = &data)
        fixed (napi_value* arraybuffer_ptr = &arraybuffer)
        fixed (nuint* offset_ptr = &offset)
        {
            return napi_get_typedarray_info(
                env,
                value,
                (nint)type_ptr,
                (nint)length_ptr,
                (nint)data_ptr,
                (nint)arraybuffer_ptr,
                (nint)offset_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_value_external(
        napi_env env,
        napi_value value,
        nint result_ptr);

    public override napi_status GetValueExternal(napi_env env, napi_value value, out nint result)
    {
        result = default;
        fixed (nint* result_ptr = &result)
        {
            return napi_get_value_external(env, value, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_strict_equals(
        napi_env env,
        napi_value lhs,
        napi_value rhs,
        nint result_ptr);

    public override napi_status StrictEquals(
        napi_env env, napi_value lhs, napi_value rhs, out bool result)
    {
        result = default;
        fixed (bool* result_ptr = &result)
        {
            return napi_strict_equals(env, lhs, rhs, (nint)result_ptr);
        }
    }

    #endregion

    #region Value creation

    [DllImport("__Internal")]
    private static extern napi_status napi_get_global(napi_env env, nint result_ptr);

    public override napi_status GetGlobal(napi_env env, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_get_global(env, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_undefined(napi_env env, nint result_ptr);

    public override napi_status GetUndefined(napi_env env, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_get_undefined(env, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_null(napi_env env, nint result_ptr);

    public override napi_status GetNull(napi_env env, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_get_null(env, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_boolean(napi_env env, c_bool value, nint result_ptr);

    public override napi_status GetBoolean(napi_env env, bool value, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_get_boolean(env, value, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_double(napi_env env, double value, nint result_ptr);

    public override napi_status CreateNumber(napi_env env, double value, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_double(env, value, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_int32(napi_env env, int value, nint result_ptr);

    public override napi_status CreateNumber(napi_env env, int value, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_int32(env, value, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_uint32(napi_env env, uint value, nint result_ptr);

    public override napi_status CreateNumber(napi_env env, uint value, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_uint32(env, value, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_int64(napi_env env, long value, nint result_ptr);

    public override napi_status CreateNumber(napi_env env, long value, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_int64(env, value, (nint)result_ptr);
        }
    }


    [DllImport("__Internal")]
    private static extern napi_status napi_create_bigint_int64(napi_env env, long value, nint result_ptr);

    public override napi_status CreateBigInt(
        napi_env env, long value, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_bigint_int64(env, value, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_bigint_uint64(napi_env env, ulong value, nint result_ptr);

    public override napi_status CreateBigInt(
        napi_env env, ulong value, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_bigint_uint64(env, value, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_bigint_words(
        napi_env env, int sign, nuint wordslen, nint words_ptr, nint result_ptr);

    public override napi_status CreateBigInt(
        napi_env env, int sign, ReadOnlySpan<ulong> words, out napi_value result)
    {
        result = default;
        if (words.Length == 0)
        {
            words = [0];
        }
        fixed (ulong* words_ptr = words)
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_bigint_words(
                env, sign, (nuint)words.Length, (nint)words_ptr, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_string_utf8(
        napi_env env, nint str_ptr, nuint strlen, nint result_ptr);

    public override napi_status CreateString(
        napi_env env, ReadOnlySpan<byte> utf8Str, out napi_value result)
    {
        result = default;
        fixed (byte* str_ptr = &utf8Str.GetPinnableReference())
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_string_utf8(
                env, (nint)str_ptr, (nuint)utf8Str.Length, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_string_utf16(
        napi_env env, nint str_ptr, nuint strlen, nint result_ptr);

    public override napi_status CreateString(
        napi_env env, ReadOnlySpan<char> utf16Str, out napi_value result)
    {
        result = default;
        fixed (char* str_ptr = &utf16Str.GetPinnableReference())
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_string_utf16(
                env, (nint)str_ptr, (nuint)utf16Str.Length, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_date(napi_env env, double value, nint result_ptr);

    public override napi_status CreateDate(napi_env env, double time, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_date(env, time, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_symbol(napi_env env, napi_value desc, nint result_ptr);

    public override napi_status CreateSymbol(
        napi_env env, napi_value description, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_symbol(env, description, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_object(napi_env env, nint result_ptr);

    public override napi_status CreateObject(napi_env env, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_object(env, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_array(napi_env env, nint result_ptr);

    public override napi_status CreateArray(napi_env env, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_array(env, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_array_with_length(napi_env env, nuint len, nint result_ptr);

    public override napi_status CreateArray(napi_env env, uint length, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_array_with_length(env, length, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_arraybuffer
        (napi_env env, nuint byte_length, nint data_ptr, nint result_ptr);

    public override napi_status CreateArrayBuffer(
        napi_env env,
        nuint byte_length,
        out nint data,
        out napi_value result)
    {
        data = default;
        result = default;
        fixed (nint* data_ptr = &data)
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_arraybuffer(
                env, byte_length, (nint)data_ptr, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_external_arraybuffer(
        napi_env env, 
        nint external_data,
        nuint byte_length,
        napi_finalize finalize_cb,
        nint finalize_hint, 
        nint result_ptr);

    public override napi_status CreateArrayBuffer(
        napi_env env,
        nint external_data,
        nuint byte_length,
        napi_finalize finalize_cb,
        nint finalize_hint,
        out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_external_arraybuffer(
                env,
                external_data,
                byte_length,
                finalize_cb,
                finalize_hint,
                (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_detach_arraybuffer(napi_env env, napi_value arraybuffer);

    public override napi_status DetachArrayBuffer(napi_env env, napi_value arraybuffer)
    {
        return napi_detach_arraybuffer(env, arraybuffer);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_typedarray(
        napi_env env,
        napi_typedarray_type type,
        nuint length,
        napi_value arraybuffer,
        nuint byte_offset,
        nint result_ptr);

    public override napi_status CreateTypedArray(
        napi_env env,
        napi_typedarray_type type,
        nuint length,
        napi_value arraybuffer,
        nuint byte_offset,
        out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_typedarray(
                env, type, length, arraybuffer, byte_offset, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_dataview(
        napi_env env,
        nuint length,
        napi_value arraybuffer,
        nuint byte_offset,
        nint result_ptr);

    public override napi_status CreateDataView(
        napi_env env,
        nuint length,
        napi_value arraybuffer,
        nuint byte_offset,
        out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_dataview(
                env, length, arraybuffer, byte_offset, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_external(
        napi_env env,
        nint data,
        napi_finalize finalize_cb,
        nint finalize_hint,
        nint result_ptr);

    public override napi_status CreateExternal(
        napi_env env,
        nint data,
        napi_finalize finalize_cb,
        nint finalize_hint,
        out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_external(env, data, finalize_cb, finalize_hint, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_function(
        napi_env env,
        nint name_ptr,
        nuint name_length,
        napi_callback cb,
        nint data,
        nint result_ptr);

    public override napi_status CreateFunction(
        napi_env env,
        string? name,
        napi_callback cb,
        nint data,
        out napi_value result)
    {
        using (PooledBuffer nameBuffer = PooledBuffer.FromStringUtf8(name))
            fixed (byte* name_ptr = nameBuffer)
            fixed (napi_value* result_ptr = &result)
            {
                return napi_create_function(
                    env,
                    (nint)name_ptr,
                    (nuint)nameBuffer.Length,
                    cb,
                    data,
                    (nint)result_ptr);
            }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_promise(
        napi_env env, nint deferred_ptr, nint promise_ptr);

    public override napi_status CreatePromise(
        napi_env env, out napi_deferred deferred, out napi_value promise)
    {
        deferred = default;
        promise = default;
        fixed (napi_deferred* deferred_ptr = &deferred)
        fixed (napi_value* result_ptr = &promise)
        {
            return napi_create_promise(env, (nint)deferred_ptr, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_resolve_deferred(
        napi_env env, napi_deferred deferred, napi_value resolution);

    public override napi_status ResolveDeferred(
        napi_env env, napi_deferred deferred, napi_value resolution)
    {
        return napi_resolve_deferred(env, deferred, resolution);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_reject_deferred(
        napi_env env, napi_deferred deferred, napi_value rejection);

    public override napi_status RejectDeferred(
        napi_env env, napi_deferred deferred, napi_value rejection)
    {
        return napi_reject_deferred(env, deferred, rejection);
    }

    #endregion

    #region Value coercion

    [DllImport("__Internal")]
    private static extern napi_status napi_coerce_to_bool(napi_env env, napi_value value, nint result_ptr);

    public override napi_status CoerceToBool(napi_env env, napi_value value, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_coerce_to_bool(env, value, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_coerce_to_number(napi_env env, napi_value value, nint result_ptr);

    public override napi_status CoerceToNumber(
        napi_env env, napi_value value, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_coerce_to_number(env, value, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_coerce_to_object(napi_env env, napi_value value, nint result_ptr);

    public override napi_status CoerceToObject(
        napi_env env, napi_value value, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_coerce_to_object(env, value, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_coerce_to_string(napi_env env, napi_value value, nint result_ptr);

    public override napi_status CoerceToString(
        napi_env env, napi_value value, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_coerce_to_string(env, value, (nint)result_ptr);
        }
    }

    #endregion

    #region Handle scopes

    [DllImport("__Internal")]
    private static extern napi_status napi_open_handle_scope(napi_env env, nint result_ptr);

    public override napi_status OpenHandleScope(napi_env env, out napi_handle_scope result)
    {
        fixed (napi_handle_scope* result_ptr = &result)
        {
            return napi_open_handle_scope(env, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_close_handle_scope(napi_env env, napi_handle_scope scope);

    public override napi_status CloseHandleScope(napi_env env, napi_handle_scope scope)
    {
        return napi_close_handle_scope(env, scope);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_open_escapable_handle_scope(napi_env env, nint result_ptr);

    public override napi_status OpenEscapableHandleScope(
        napi_env env,
        out napi_escapable_handle_scope result)
    {
        fixed (napi_escapable_handle_scope* result_ptr = &result)
        {
            return napi_open_escapable_handle_scope(env, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_close_escapable_handle_scope(
        napi_env env, napi_escapable_handle_scope scope);

    public override napi_status CloseEscapableHandleScope(
        napi_env env, napi_escapable_handle_scope scope)
    {
        return napi_close_escapable_handle_scope(env, scope);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_escape_handle(
        napi_env env,
        napi_escapable_handle_scope scope,
        napi_value escapee,
        nint result_ptr);

    public override napi_status EscapeHandle(
        napi_env env,
        napi_escapable_handle_scope scope,
        napi_value escapee,
        out napi_value result)
    {
        fixed (napi_value* result_ptr = &result)
        {
            return napi_escape_handle(env, scope, escapee, (nint)result_ptr);
        }
    }

    #endregion

    #region References

    [DllImport("__Internal")]
    private static extern napi_status napi_create_reference(
        napi_env env,
        napi_value value,
        uint initialRefcount,
        nint result_ptr);

    public override napi_status CreateReference(
        napi_env env,
        napi_value value,
        uint initialRefcount,
        out napi_ref result)
    {
        fixed (napi_ref* result_ptr = &result)
        {
            return napi_create_reference(env, value, initialRefcount, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_delete_reference(napi_env env, napi_ref @ref);

    public override napi_status DeleteReference(napi_env env, napi_ref @ref)
    {
        return napi_delete_reference(env, @ref);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_reference_ref(napi_env env, napi_ref @ref, nint result_ptr);

    public override napi_status RefReference(napi_env env, napi_ref @ref, out uint result)
    {
        fixed (uint* result_ptr = &result)
        {
            return napi_reference_ref(env, @ref, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_reference_unref(napi_env env, napi_ref @ref, nint result_ptr);

    public override napi_status UnrefReference(napi_env env, napi_ref @ref, out uint result)
    {
        fixed (uint* result_ptr = &result)
        {
            return napi_reference_unref(env, @ref, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_reference_value(napi_env env, napi_ref @ref, nint result_ptr);

    public override napi_status GetReferenceValue(
        napi_env env, napi_ref @ref, out napi_value result)
    {
        fixed (napi_value* result_ptr = &result)
        {
            return napi_get_reference_value(env, @ref, (nint)result_ptr);
        }
    }

    #endregion

    #region Function calls

    [DllImport("__Internal")]
    private static extern napi_status napi_call_function(
        napi_env env,
        napi_value recv,
        napi_value func,
        nuint argc,
        nint argv_ptr,
        nint result_ptr);

    public override napi_status CallFunction(
        napi_env env,
        napi_value recv,
        napi_value func,
        ReadOnlySpan<napi_value> args,
        out napi_value result)
    {
        nuint argc = (nuint)args.Length;
        result = default;
        fixed (napi_value* argv_ptr = &args.GetPinnableReference())
        fixed (napi_value* result_ptr = &result)
        {
            return napi_call_function(
                env, recv, func, argc, (nint)argv_ptr, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_cb_info(
        napi_env env,
        napi_callback_info cbinfo,
        nint argc_ptr,
        nint argv_ptr,
        nint this_ptr,
        nint data_ptr);

    public override napi_status GetCallbackInfo(
        napi_env env,
        napi_callback_info cbinfo,
        out nuint argc,
        out nint data)
    {
        argc = default;
        data = default;
        fixed (nuint* argc_ptr = &argc)
        fixed (nint* data_ptr = &data)
        {
            return napi_get_cb_info(env, cbinfo, (nint)argc_ptr, default, default, (nint)data_ptr);
        }
    }

    public override napi_status GetCallbackArgs(
        napi_env env,
        napi_callback_info cbinfo,
        Span<napi_value> args,
        out napi_value this_arg)
    {
        nint argc = args.Length;
        nint* argc_ptr = &argc;
        this_arg = default;
        fixed (napi_value* argv_ptr = &args.GetPinnableReference())
        fixed (napi_value* this_ptr = &this_arg)
        {
            return napi_get_cb_info(
                env, cbinfo, (nint)argc_ptr, (nint)argv_ptr, (nint)this_ptr, default);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_new_target(
        napi_env env,
        napi_callback_info cbinfo,
        nint result_ptr);

    public override napi_status GetNewTarget(
        napi_env env,
        napi_callback_info cbinfo,
        out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_get_new_target(env, cbinfo, (nint)result_ptr);
        }
    }

    #endregion

    #region Object properties

    [DllImport("__Internal")]
    private static extern napi_status napi_has_property(
        napi_env env, napi_value js_object, napi_value key, nint result_ptr);

    public override napi_status HasProperty(
        napi_env env, napi_value js_object, napi_value key, out bool result)
    {
        result = default;
        fixed (bool* result_ptr = &result)
        {
            return napi_has_property(env, js_object, key, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_has_own_property(
        napi_env env, napi_value js_object, napi_value key, nint result_ptr);

    public override napi_status HasOwnProperty(
        napi_env env, napi_value js_object, napi_value key, out bool result)
    {
        result = default;
        fixed (bool* result_ptr = &result)
        {
            return napi_has_own_property(env, js_object, key, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_property(
        napi_env env, napi_value js_object, napi_value key, nint result_ptr);

    public override napi_status GetProperty(
        napi_env env, napi_value js_object, napi_value key, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_get_property(env, js_object, key, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_set_property(
        napi_env env, napi_value js_object, napi_value key, napi_value value);

    public override napi_status SetProperty(
        napi_env env, napi_value js_object, napi_value key, napi_value value)
    {
        return napi_set_property(env, js_object, key, value);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_delete_property(
        napi_env env, napi_value js_object, napi_value key, nint result_ptr);

    public override napi_status DeleteProperty(
        napi_env env, napi_value js_object, napi_value key, out bool result)
    {
        result = default;
        fixed (bool* result_ptr = &result)
        {
            return napi_delete_property(env, js_object, key, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_has_named_property(
        napi_env env, napi_value js_object, nint name_ptr, nint result_ptr);

    public override napi_status HasNamedProperty(
        napi_env env, napi_value js_object, ReadOnlySpan<byte> utf8name, out bool result)
    {
        result = default;
        fixed (byte* name_ptr = &utf8name.GetPinnableReference())
        fixed (bool* result_ptr = &result)
        {
            return napi_has_named_property(env, js_object, (nint)name_ptr, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_named_property(
        napi_env env, napi_value js_object, nint name_ptr, nint result_ptr);

    public override napi_status GetNamedProperty(
        napi_env env, napi_value js_object, ReadOnlySpan<byte> utf8name, out napi_value result)
    {
        result = default;
        fixed (byte* name_ptr = &utf8name.GetPinnableReference())
        fixed (napi_value* result_ptr = &result)
        {
            return napi_get_named_property(env, js_object, (nint)name_ptr, (nint)result_ptr);
        }
    }


    [DllImport("__Internal")]
    private static extern napi_status napi_set_named_property(
        napi_env env, napi_value js_object, nint name_ptr, napi_value value);

    public override napi_status SetNamedProperty(
        napi_env env, napi_value js_object, ReadOnlySpan<byte> utf8name, napi_value value)
    {
        fixed (byte* name_ptr = &utf8name.GetPinnableReference())
        {
            return napi_set_named_property(env, js_object, (nint)name_ptr, value);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_has_element(
        napi_env env, napi_value js_object, uint index, nint result_ptr);

    public override napi_status HasElement(
        napi_env env, napi_value js_object, uint index, out bool result)
    {
        result = default;
        fixed (bool* result_ptr = &result)
        {
            return napi_has_element(env, js_object, index, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_element(
        napi_env env, napi_value js_object, uint index, nint result_ptr);

    public override napi_status GetElement(
        napi_env env, napi_value js_object, uint index, out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_get_element(env, js_object, index, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_set_element(
        napi_env env, napi_value js_object, uint index, napi_value value);

    public override napi_status SetElement(
        napi_env env, napi_value js_object, uint index, napi_value value)
    {
        return napi_set_element(env, js_object, index, value);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_delete_element(
        napi_env env, napi_value js_object, uint index, nint result_ptr);

    public override napi_status DeleteElement(
        napi_env env, napi_value js_object, uint index, out bool result)
    {
        result = default;
        fixed (bool* result_ptr = &result)
        {
            return napi_delete_element(env, js_object, index, (nint)result_ptr);
        }
    }

    #endregion

    #region Property and class definition

    [DllImport("__Internal")]
    private static extern napi_status napi_get_property_names(
        napi_env env, napi_value js_object, nint result_ptr);

    public override napi_status GetPropertyNames(
        napi_env env,
        napi_value js_object,
        out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_get_property_names(env, js_object, (nint)result_ptr);
        }
    }


    [DllImport("__Internal")]
    private static extern napi_status napi_get_all_property_names(
        napi_env env,
        napi_value js_object,
        napi_key_collection_mode key_mode,
        napi_key_filter key_filter,
        napi_key_conversion key_conversion,
        nint result_ptr);

    public override napi_status GetAllPropertyNames(
        napi_env env,
        napi_value js_object,
        napi_key_collection_mode key_mode,
        napi_key_filter key_filter,
        napi_key_conversion key_conversion,
        out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_get_all_property_names(
                env, js_object, key_mode, key_filter, key_conversion, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_object_freeze(napi_env env, napi_value value);

    public override napi_status Freeze(napi_env env, napi_value value)
    {
        return napi_object_freeze(env, value);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_object_seal(napi_env env, napi_value value);

    public override napi_status Seal(napi_env env, napi_value value)
    {
        return napi_object_seal(env, value);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_define_properties(
        napi_env env,
        napi_value js_object,
        nuint count,
        nint properties_ptr);

    public override napi_status DefineProperties(
        napi_env env,
        napi_value js_object,
        ReadOnlySpan<napi_property_descriptor> properties)
    {
        fixed (napi_property_descriptor* properties_ptr = &properties.GetPinnableReference())
        {
            return napi_define_properties(
                env, js_object, (nuint)properties.Length, (nint)properties_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_define_class(
        napi_env env,
        nint name_ptr,
        nuint name_length,
        napi_callback constructor,
        nint data,
        nuint properties_length,
        nint properties_ptr,
        nint result_ptr);

    public override napi_status DefineClass(
        napi_env env,
        string name,
        napi_callback constructor,
        nint data,
        ReadOnlySpan<napi_property_descriptor> properties,
        out napi_value result)
    {
        result = default;
        using (PooledBuffer nameBuffer = PooledBuffer.FromStringUtf8(name))
            fixed (byte* name_ptr = nameBuffer)
            fixed (napi_property_descriptor* properties_ptr = &properties.GetPinnableReference())
            fixed (napi_value* result_ptr = &result)
            {
                return napi_define_class(
                    env,
                    (nint)name_ptr,
                    (nuint)nameBuffer.Length,
                    constructor,
                    data,
                    (nuint)properties.Length,
                    (nint)properties_ptr,
                    (nint)result_ptr);
            }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_prototype(
        napi_env env,
        napi_value js_object,
        nint result_ptr);

    public override napi_status GetPrototype(
        napi_env env,
        napi_value js_object,
        out napi_value result)
    {
        result = default;
        fixed (napi_value* result_ptr = &result)
        {
            return napi_get_prototype(env, js_object, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_new_instance(
        napi_env env,
        napi_value constructor,
        nuint argc,
        nint args_ptr,
        nint result_ptr);

    public override napi_status NewInstance(
        napi_env env,
        napi_value constructor,
        ReadOnlySpan<napi_value> args,
        out napi_value result)
    {
        result = default;
        fixed (napi_value* args_ptr = &args.GetPinnableReference())
        fixed (napi_value* result_ptr = &result)
        {
            return napi_new_instance(
                env, constructor, (nuint)args.Length, (nint)args_ptr, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_instanceof(
        napi_env env,
        napi_value js_object,
        napi_value constructor,
        nint result_ptr);

    public override napi_status InstanceOf(
        napi_env env,
        napi_value js_object,
        napi_value constructor,
        out bool result)
    {
        result = default;
        fixed (bool* result_ptr = &result)
        {
            return napi_instanceof(env, js_object, constructor, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_wrap(
        napi_env env,
        napi_value js_object,
        nint native_object,
        napi_finalize finalize_cb,
        nint finalize_hint,
        nint result_ptr);

    public override napi_status Wrap(
        napi_env env,
        napi_value js_object,
        nint native_object,
        napi_finalize finalize_cb,
        nint finalize_hint,
        out napi_ref result)
    {
        // The wrapper reference must be deleted by user code.
        result = default;
        fixed (napi_ref* result_ptr = &result)
        {
            return napi_wrap(
                env, js_object, native_object, finalize_cb, finalize_hint, (nint)result_ptr);
        }
    }

    public override napi_status Wrap(
        napi_env env,
        napi_value js_object,
        nint native_object,
        napi_finalize finalize_cb,
        nint finalize_hint)
    {
        // The wrapper reference is deleted by Node.js.
        return napi_wrap(
            env, js_object, native_object, finalize_cb, finalize_hint, default);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_unwrap(napi_env env, napi_value js_object, nint result_ptr);

    public override napi_status Unwrap(napi_env env, napi_value js_object, out nint result)
    {
        result = default;
        fixed (nint* result_ptr = &result)
        {
            return napi_unwrap(env, js_object, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_remove_wrap(napi_env env, napi_value js_object, nint result_ptr);

    public override napi_status RemoveWrap(napi_env env, napi_value js_object, out nint result)
    {
        result = default;
        fixed (nint* result_ptr = &result)
        {
            return napi_remove_wrap(env, js_object, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_type_tag_object(napi_env env, napi_value js_object, nint result_ptr);

    public override napi_status SetObjectTypeTag(
        napi_env env, napi_value value, Guid typeTag)
    {
        Guid* tag_ptr = &typeTag;
        return napi_type_tag_object(
            env, value, (nint)tag_ptr);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_check_object_type_tag(
        napi_env env, napi_value value, nint tag_ptr, nint result_ptr);

    public override napi_status CheckObjectTypeTag(
        napi_env env, napi_value value, Guid typeTag, out bool result)
    {
        result = default;
        Guid* tag_ptr = &typeTag;
        fixed (bool* result_ptr = &result)
        {
            return napi_check_object_type_tag(
                env, value, (nint)tag_ptr, (nint)result_ptr);
        }
    }

    #endregion

#pragma warning restore IDE1006
}
