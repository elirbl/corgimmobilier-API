import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { Link } from 'react-router-dom';
import { AuthLayout } from '../../components/auth/AuthLayout';
import { FormError } from '../../components/form/FormError';
import { InputField } from '../../components/form/InputField';
import { PasswordInput } from '../../components/form/PasswordInput';
import { useLogin } from '../../hooks/useAuth';
import { loginSchema, type LoginFormValues } from '../../schemas/authSchemas';

export default function LoginPage() {
  const login = useLogin();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginFormValues>({
    resolver: zodResolver(loginSchema),
  });

  function onSubmit(values: LoginFormValues) {
    login.mutate(values);
  }

  return (
    <AuthLayout title="Connexion" subtitle="Accédez à votre espace Ymmo">
      <form onSubmit={handleSubmit(onSubmit)} noValidate>
        <InputField
          label="Email"
          type="email"
          autoComplete="email"
          required
          error={errors.email?.message}
          {...register('email')}
        />

        <PasswordInput
          label="Mot de passe"
          autoComplete="current-password"
          required
          error={errors.password?.message}
          {...register('password')}
        />

        <div className="mb-4 text-right">
          <Link
            to="/forgot-password"
            className="text-sm font-medium text-brand-600 hover:text-brand-700 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-brand-500 focus-visible:ring-offset-1 rounded"
          >
            Mot de passe oublié ?
          </Link>
        </div>

        <FormError
          message={
            login.isError
              ? 'Email ou mot de passe incorrect. Veuillez réessayer.'
              : undefined
          }
        />

        <button
          type="submit"
          disabled={login.isPending}
          className="mt-2 w-full rounded-md bg-brand-600 px-4 py-2 text-sm font-semibold text-white shadow-sm transition-colors hover:bg-brand-700 focus:outline-none focus-visible:ring-2 focus-visible:ring-brand-500 focus-visible:ring-offset-2 disabled:opacity-60"
        >
          {login.isPending ? 'Connexion...' : 'Se connecter'}
        </button>
      </form>

      <p className="mt-6 text-center text-sm text-navy-400">
        Pas encore de compte ?{' '}
        <Link
          to="/register"
          className="font-medium text-brand-600 hover:text-brand-700 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-brand-500 focus-visible:ring-offset-1 rounded"
        >
          Créer un compte
        </Link>
      </p>
    </AuthLayout>
  );
}
