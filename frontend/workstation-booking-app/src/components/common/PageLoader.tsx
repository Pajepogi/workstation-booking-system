export default function PageLoader() {
  return (
    <div className="flex min-h-screen items-center justify-center bg-slate-100">
      <div className="flex flex-col items-center gap-4 rounded-xl bg-white p-8 shadow-sm">
        <div className="h-10 w-10 animate-spin rounded-full border-4 border-blue-600 border-t-transparent" />

        <div className="text-center">
          <p className="text-sm font-medium text-slate-700">
            Checking authentication
          </p>

          <p className="mt-1 text-xs text-slate-500">
            Please wait while we verify your Windows account.
          </p>
        </div>
      </div>
    </div>
  );
}
