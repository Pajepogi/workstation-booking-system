export default function AccessDenied() {
  return (
    <div className="flex min-h-screen items-center justify-center bg-slate-100 p-6">
      <div className="w-full max-w-md rounded-xl bg-white p-6 text-center shadow-sm">
        <div className="mx-auto mb-4 flex h-12 w-12 items-center justify-center rounded-full bg-red-100">
          <span className="text-xl font-bold text-red-600">!</span>
        </div>

        <h1 className="text-xl font-bold text-slate-900">Access Denied</h1>

        <p className="mt-2 text-sm text-slate-500">
          You are not authenticated or your Windows account could not be
          verified.
        </p>

        <p className="mt-4 text-xs text-slate-400">
          Please contact your system administrator if this problem continues.
        </p>
      </div>
    </div>
  );
}
