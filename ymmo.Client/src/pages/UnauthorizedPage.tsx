import { Link } from 'react-router-dom';

export default function UnauthorizedPage() {
  return (
    <div className="flex min-h-screen flex-col items-center justify-center gap-4 bg-gray-50">
      <h1 className="text-2xl font-semibold text-gray-800">Accès refusé</h1>
      <p className="text-gray-600">Vous n'avez pas les droits nécessaires pour accéder à cette page.</p>
      <Link to="/" className="text-blue-600 hover:underline">
        Retour au tableau de bord
      </Link>
    </div>
  );
}
