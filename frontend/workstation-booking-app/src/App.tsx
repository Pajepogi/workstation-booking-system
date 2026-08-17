import { useState } from "react";
import { Toaster } from "react-hot-toast"; // 1. Import Toaster
import DashboardPage from "./pages/Dashboard";
import LoginPage from "./pages/LoginPage";
import {
  getCurrentUser,
  logout,
  type AuthenticatedUser,
} from "./services/userService";

function App() {
  const [currentUser, setCurrentUser] = useState<AuthenticatedUser | null>(
    getCurrentUser(),
  );

  const handleLoginSuccess = () => {
    setCurrentUser(getCurrentUser());
  };

  const handleLogout = () => {
    logout();
    setCurrentUser(null);
  };

  return (
    <>
      {/* 2. Mount Toaster here so notifications display anywhere in the app */}
      <Toaster
        position="top-right"
        toastOptions={{
          duration: 4000,
          style: {
            zIndex: 9999, // Ensures toasts display over modals
          },
        }}
      />

      {!currentUser?.isAuthenticated ? (
        <LoginPage onLoginSuccess={handleLoginSuccess} />
      ) : (
        <DashboardPage currentUser={currentUser} onLogout={handleLogout} />
      )}
    </>
  );
}

export default App;
