// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.JavaScript.NodeApi.Runtime;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

// Imports Node.js native APIs defined in node_api.h
public unsafe partial class NodejsRuntime
{
#pragma warning disable IDE1006 // Naming: missing prefix '_'

    #region Thread-safe functions

    [DllImport("__Internal")]
    private static extern napi_status napi_create_threadsafe_function(
        napi_env env,
        napi_value func,
        napi_value asyncResource,
        napi_value asyncResourceName,
        nuint maxQueueSize,
        nuint initialThreadCount,
        nint threadFinalizeData,
        napi_finalize threadFinalizeCallback,
        nint context,
        napi_threadsafe_function_call_js callJSCallback,
        nint result_ptr);

    public override napi_status CreateThreadSafeFunction(
        napi_env env,
        napi_value func,
        napi_value asyncResource,
        napi_value asyncResourceName,
        int maxQueueSize,
        int initialThreadCount,
        nint threadFinalizeData,
        napi_finalize threadFinalizeCallback,
        nint context,
        napi_threadsafe_function_call_js callJSCallback,
        out napi_threadsafe_function result)
    {
        fixed (napi_threadsafe_function* result_ptr = &result)
        {
            return napi_create_threadsafe_function(
                env,
                func,
                asyncResource,
                asyncResourceName,
                (nuint)maxQueueSize,
                (nuint)initialThreadCount,
                threadFinalizeData,
                threadFinalizeCallback,
                context,
                callJSCallback,
                (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_call_threadsafe_function(
        napi_threadsafe_function func,
        nint data,
        napi_threadsafe_function_call_mode isBlocking);

    public override napi_status CallThreadSafeFunction(
        napi_threadsafe_function func,
        nint data,
        napi_threadsafe_function_call_mode isBlocking)
    {
        return napi_call_threadsafe_function(func, data, isBlocking);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_threadsafe_function_context(
        napi_threadsafe_function func,
        nint result_ptr);

    public override napi_status GetThreadSafeFunctionContext(
        napi_threadsafe_function func,
        out nint result)
    {
        fixed (nint* result_ptr = &result)
        {
            return napi_get_threadsafe_function_context(func, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_acquire_threadsafe_function(napi_threadsafe_function func);

    public override napi_status AcquireThreadSafeFunction(napi_threadsafe_function func)
    {
        return napi_acquire_threadsafe_function(func);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_release_threadsafe_function(
        napi_threadsafe_function func,
        napi_threadsafe_function_release_mode mode);

    public override napi_status ReleaseThreadSafeFunction(
        napi_threadsafe_function func,
        napi_threadsafe_function_release_mode mode)
    {
        return napi_release_threadsafe_function(func, mode);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_ref_threadsafe_function(napi_env env, napi_threadsafe_function func);

    public override napi_status RefThreadSafeFunction(napi_env env, napi_threadsafe_function func)
    {
        return napi_ref_threadsafe_function(env, func);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_unref_threadsafe_function(napi_env env, napi_threadsafe_function func);

    public override napi_status UnrefThreadSafeFunction(napi_env env, napi_threadsafe_function func)
    {
        return napi_unref_threadsafe_function(env, func);
    }

    #endregion

    #region Async work

    [DllImport("__Internal")]
    private static extern napi_status napi_async_init(
        napi_env env,
        napi_value asyncResource,
        napi_value asyncResourceName,
        nint result_ptr);

    public override napi_status AsyncInit(
        napi_env env,
        napi_value asyncResource,
        napi_value asyncResourceName,
        out napi_async_context result)
    {
        fixed (napi_async_context* result_ptr = &result)
        {
            return napi_async_init(
                env,
                asyncResource,
                asyncResourceName,
                (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_async_destroy(napi_env env, napi_async_context asyncContext);

    public override napi_status AsyncDestroy(napi_env env, napi_async_context asyncContext)
    {
        return napi_async_destroy(env, asyncContext);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_async_work(
        napi_env env,
        napi_value asyncResource,
        napi_value asyncResourceName,
        napi_async_execute_callback execute,
        napi_async_complete_callback complete,
        nint data,
        nint result_ptr);

    public override napi_status CreateAsyncWork(
        napi_env env,
        napi_value asyncResource,
        napi_value asyncResourceName,
        napi_async_execute_callback execute,
        napi_async_complete_callback complete,
        nint data,
        out napi_async_work result)
    {
        fixed (napi_async_work* result_ptr = &result)
        {
            return napi_create_async_work(
                env,
                asyncResource,
                asyncResourceName,
                execute,
                complete,
                data,
                (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_queue_async_work(napi_env env, napi_async_work work);

    public override napi_status QueueAsyncWork(napi_env env, napi_async_work work)
    {
        return napi_queue_async_work(env, work);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_delete_async_work(napi_env env, napi_async_work work);

    public override napi_status DeleteAsyncWork(napi_env env, napi_async_work work)
    {
        return napi_delete_async_work(env, work);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_cancel_async_work(napi_env env, napi_async_work work);

    public override napi_status CancelAsyncWork(napi_env env, napi_async_work work)
    {
        return napi_cancel_async_work(env, work);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_make_callback(
        napi_env env,
        napi_async_context asyncContext,
        napi_value recv,
        napi_value func,
        nuint argc,
        nint args_ptr,
        nint result_ptr);

    public override napi_status MakeCallback(
        napi_env env,
        napi_async_context asyncContext,
        napi_value recv,
        napi_value func,
        Span<napi_value> args,
        out napi_value result)
    {
        fixed (napi_value* args_ptr = &args.GetPinnableReference())
        fixed (napi_value* result_ptr = &result)
        {
            return napi_make_callback(
                env,
                asyncContext,
                recv,
                func,
                (nuint)args.Length,
                (nint)args_ptr,
                (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_open_callback_scope(
        napi_env env,
        napi_value resourceObject,
        napi_async_context asyncContext,
        nint result_ptr);

    public override napi_status OpenCallbackScope(
        napi_env env,
        napi_value resourceObject,
        napi_async_context asyncContext,
        out napi_callback_scope result)
    {
        fixed (napi_callback_scope* result_ptr = &result)
        {
            return napi_open_callback_scope(
                env,
                resourceObject,
                asyncContext,
                (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_close_callback_scope(napi_env env, napi_callback_scope scope);

    public override napi_status CloseCallbackScope(napi_env env, napi_callback_scope scope)
    {
        return napi_close_callback_scope(env, scope);
    }

    #endregion

    #region Cleanup hooks

    [DllImport("__Internal")]
    private static extern napi_status napi_add_async_cleanup_hook(
        napi_env env,
        napi_async_cleanup_hook hook,
        nint arg,
        nint result_ptr);

    public override napi_status AddAsyncCleanupHook(
        napi_env env,
        napi_async_cleanup_hook hook,
        nint arg,
        out napi_async_cleanup_hook_handle result)
    {
        fixed (napi_async_cleanup_hook_handle* result_ptr = &result)
        {
            return napi_add_async_cleanup_hook(
                env,
                hook,
                arg,
                (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_remove_async_cleanup_hook(napi_async_cleanup_hook_handle removeHandle);

    public override napi_status RemoveAsyncCleanupHook(napi_async_cleanup_hook_handle removeHandle)
    {
        return napi_remove_async_cleanup_hook(removeHandle);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_add_env_cleanup_hook(napi_env env, napi_cleanup_hook func, nint arg);

    public override napi_status AddEnvCleanupHook(napi_env env, napi_cleanup_hook func, nint arg)
    {
        return napi_add_env_cleanup_hook(env, func, arg);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_remove_env_cleanup_hook(napi_env env, napi_cleanup_hook func, nint arg);

    public override napi_status RemoveEnvCleanupHook(napi_env env, napi_cleanup_hook func, nint arg)
    {
        return napi_remove_env_cleanup_hook(env, func, arg);
    }

    #endregion

    #region Buffers

    [DllImport("__Internal")]
    private static extern napi_status napi_is_buffer(napi_env env, napi_value value, nint result_ptr);

    public override napi_status IsBuffer(napi_env env, napi_value value, out bool result)
    {
        c_bool resultBool = default;
        napi_status status = napi_is_buffer(env, value, (nint)(&resultBool));
        result = (bool)resultBool;
        return status;
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_buffer(napi_env env, nuint datalen, nint data_ptr, nint result_ptr);

    public override napi_status CreateBuffer(napi_env env, Span<byte> data, out napi_value result)
    {
        fixed (byte* data_ptr = &data.GetPinnableReference())
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_buffer(
                env,
                (nuint)data.Length,
                (nint)data_ptr,
                (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_buffer_copy(
        napi_env env, nuint datalen, nint data_ptr, nint resultData_ptr, nint result_ptr);

    public override napi_status CreateBufferCopy(
        napi_env env,
        ReadOnlySpan<byte> data,
        out nint resultData,
        out napi_value result)
    {
        fixed (byte* data_ptr = &data.GetPinnableReference())
        fixed (nint* resultData_ptr = &resultData)
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_buffer_copy(
                env,
                (nuint)data.Length,
                (nint)data_ptr,
                (nint)resultData_ptr,
                (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_create_external_buffer(
        napi_env env,
        nuint data_len,
        nint data_ptr,
        napi_finalize finalizeCallback,
        nint finalizeHint,
        nint result_ptr);

    public override napi_status CreateExternalBuffer(
        napi_env env,
        Span<byte> data,
        napi_finalize finalizeCallback,
        nint finalizeHint,
        out napi_value result)
    {
        fixed (byte* data_ptr = &data.GetPinnableReference())
        fixed (napi_value* result_ptr = &result)
        {
            return napi_create_external_buffer(
                env,
                (nuint)data.Length,
                (nint)data_ptr,
                finalizeCallback,
                finalizeHint,
                (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_buffer_info(
        napi_env env,
        napi_value value,
        nint data_ptr,
        nint length_ptr);

    public override napi_status GetBufferInfo(
        napi_env env,
        napi_value value,
        out nint data,
        out nuint length)
    {
        fixed (nint* data_ptr = &data)
        fixed (nuint* length_ptr = &length)
        {
            return napi_get_buffer_info(
                env,
                value,
                (nint)data_ptr,
                (nint)length_ptr);
        }
    }

    #endregion

    #region Misc Node.js functions

    [DllImport("__Internal")]
    private static extern void napi_fatal_error(nint location_ptr, nuint loclen, nint message_ptr, nuint msglen);

    [DoesNotReturn]
    public override void FatalError(string location, string message)
    {
        using (PooledBuffer locationBuffer = PooledBuffer.FromStringUtf8(location))
        using (PooledBuffer messageBuffer = PooledBuffer.FromStringUtf8(message))
            fixed (byte* location_ptr = locationBuffer)
            fixed (byte* message_ptr = messageBuffer)
            {
                napi_fatal_error(
                    (nint)location_ptr,
                    (nuint)locationBuffer.Length,
                    (nint)message_ptr,
                    (nuint)messageBuffer.Length);
                throw new Exception("napi_fatal_error() returned.");
            }
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_fatal_exception(napi_env env, napi_value err);

    public override napi_status FatalException(napi_env env, napi_value err)
    {
        return napi_fatal_exception(env, err);
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_uv_event_loop(napi_env env, nint result_ptr);

    public override napi_status GetUVEventLoop(napi_env env, out uv_loop_t result)
    {
        fixed (uv_loop_t* result_ptr = &result)
        {
            return napi_get_uv_event_loop(env, (nint)result_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern void napi_module_register(nint module_ptr);

    public override void RegisterModule(ref napi_module module)
    {
        fixed (napi_module* module_ptr = &module)
        {
            napi_module_register((nint)module_ptr);
        }
    }

    [DllImport("__Internal")]
    private static extern napi_status node_api_get_module_file_name(napi_env env, nint result_ptr_ptr);

    public override napi_status GetModuleFileName(napi_env env, out string result)
    {
        byte* result_ptr = null;
        byte** result_ptr_ptr = &result_ptr;
        napi_status status = node_api_get_module_file_name(env, (nint)result_ptr_ptr);
        result = (status == napi_status.napi_ok ? PtrToStringUTF8(result_ptr) : null)!;
        return status;
    }

    [DllImport("__Internal")]
    private static extern napi_status napi_get_node_version(napi_env env, nint result_ptr);

    public override napi_status GetNodeVersion(napi_env env, out napi_node_version result)
    {
        fixed (napi_node_version* result_ptr = &result)
        {
            return napi_get_node_version(env, (nint)result_ptr);
        }
    }

    #endregion

#pragma warning restore IDE1006
}
