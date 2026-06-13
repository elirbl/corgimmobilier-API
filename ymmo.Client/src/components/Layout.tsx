import { NavLink, Outlet } from 'react-router-dom';
import { useAuthStore } from '../stores/authStore';
import { useUIStore } from '../stores/uiStore';
import { useLogout } from '../hooks/useAuth';

const navItems: { to: string; label: string; roles?: Array<'Admin' | 'Agent' | 'Client'> }[] = [
  { to: '/', label: 'Tableau de bord' },
  { to: '/properties', label: 'Biens' },
  { to: '/agencies', label: 'Agences', roles: ['Admin'] },
];

export function Layout() {
  const { user } = useAuthStore();
  const { sidebarOpen, toggleSidebar } = useUIStore();
  const logout = useLogout();

  return (
    <div className="flex min-h-screen bg-gray-50">
      <aside
        className={`${sidebarOpen ? 'w-64' : 'w-0'} overflow-hidden bg-white border-r border-gray-200 transition-all duration-200`}
      >
        <div className="p-4 text-lg font-semibold text-gray-800">Ymmo</div>
        <nav className="flex flex-col gap-1 px-2">
          {navItems
            .filter((item) => !item.roles || (user && item.roles.includes(user.role)))
            .map((item) => (
              <NavLink
                key={item.to}
                to={item.to}
                className={({ isActive }) =>
                  `rounded px-3 py-2 text-sm font-medium ${
                    isActive ? 'bg-blue-100 text-blue-700' : 'text-gray-600 hover:bg-gray-100'
                  }`
                }
              >
                {item.label}
              </NavLink>
            ))}
        </nav>
      </aside>

      <div className="flex flex-1 flex-col">
        <header className="flex items-center justify-between border-b border-gray-200 bg-white px-4 py-3">
          <button
            type="button"
            onClick={toggleSidebar}
            className="rounded px-2 py-1 text-sm text-gray-600 hover:bg-gray-100"
          >
            ☰
          </button>

          <div className="flex items-center gap-3">
            {user && (
              <span className="text-sm text-gray-700">
                {user.firstName} {user.lastName} · {user.role}
              </span>
            )}
            <button
              type="button"
              onClick={() => logout.mutate()}
              className="rounded bg-gray-100 px-3 py-1.5 text-sm font-medium text-gray-700 hover:bg-gray-200"
            >
              Déconnexion
            </button>
          </div>
        </header>

        <main className="flex-1 p-6">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
