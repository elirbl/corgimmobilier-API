import { useAuthStore } from '../stores/authStore';

export default function DashboardPage() {
  const { user } = useAuthStore();

  return (
    <div>
      <h1 className="mb-4 text-2xl font-semibold text-gray-800">Tableau de bord</h1>
      <p className="text-gray-600">
        Bienvenue {user?.firstName} ({user?.role}).
      </p>
    </div>
  );
}
