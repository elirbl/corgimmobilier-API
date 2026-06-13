import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { Link } from 'react-router-dom';
import { AuthLayout } from '../../components/auth/AuthLayout';
import { FormError } from '../../components/form/FormError';
import { InputField } from '../../components/form/InputField';
import { useForgotPassword } from '../../hooks/useAuth';
import { forgotPasswordSchema, type ForgotPasswordFormValues } from '../../schemas/authSchemas';

export default function ForgotPasswordPage() {
  const forgotPassword = useForgotPassword();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<ForgotPasswordFormValues>({
    resolver: zodResolver(forgotPasswordSchema),
  });

  function onSubmit(values: ForgotPasswordFormValues) {
    forgotPassword.mutate(values);
  }

  return (
    <AuthLayout
      title="Mot de passe oublié"
      subtitle="Recevez un lien de réinitialisation par email"
    >
      {forgotPassword.isSuccess ? (
        <div role="status" className="text-center">
          <p className="text-sm text-navy-800">
            Si un compte existe avec cette adresse, un email contenant les instructions de
            réinitialisation a été envoyé.
          </p>
          <Link
            to="/login"
            className="mt-6 inline-block text-sm font-medium text-brand-600 hover:text-brand-700 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-brand-500 focus-visible:ring-offset-1 rounded"
          >
            Retour à la connexion
          </Link>
        </div>
      ) : (
        <form onSubmit={handleSubmit(onSubmit)} noValidate>
          <InputField
            label="Email"
            type="email"
            autoComplete="email"
            required
            error={errors.email?.message}
            {...register('email')}
          />

          <FormError
            message={
              forgotPassword.isError
                ? 'Une erreur est survenue. Veuillez réessayer.'
                : undefined
            }
          />

          <button
            type="submit"
            disabled={forgotPassword.isPending}
            className="mt-2 w-full rounded-md bg-brand-600 px-4 py-2 text-sm font-semibold text-white shadow-sm transition-colors hover:bg-brand-700 focus:outline-none focus-visible:ring-2 focus-visible:ring-brand-500 focus-visible:ring-offset-2 disabled:opacity-60"
          >
            {forgotPassword.isPending ? 'Envoi...' : 'Envoyer le lien'}
          </button>

          <p className="mt-6 text-center text-sm text-navy-400">
            <Link
              to="/login"
              className="font-medium text-brand-600 hover:text-brand-700 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-brand-500 focus-visible:ring-offset-1 rounded"
            >
              Retour à la connexion
            </Link>
          </p>
        </form>
      )}
    </AuthLayout>
  );
}
