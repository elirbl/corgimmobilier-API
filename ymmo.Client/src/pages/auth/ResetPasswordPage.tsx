import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { Link, useSearchParams } from 'react-router-dom';
import { AuthLayout } from '../../components/auth/AuthLayout';
import { FormError } from '../../components/form/FormError';
import { PasswordInput } from '../../components/form/PasswordInput';
import { useResetPassword } from '../../hooks/useAuth';
import { resetPasswordSchema, type ResetPasswordFormValues } from '../../schemas/authSchemas';

export default function ResetPasswordPage() {
  const [searchParams] = useSearchParams();
  const token = searchParams.get('token') ?? '';
  const resetPassword = useResetPassword();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<ResetPasswordFormValues>({
    resolver: zodResolver(resetPasswordSchema),
  });

  function onSubmit(values: ResetPasswordFormValues) {
    resetPassword.mutate({ token, password: values.password });
  }

  if (!token) {
    return (
      <AuthLayout title="Lien invalide" subtitle="Réinitialisation du mot de passe">
        <div role="alert" className="text-center">
          <p className="text-sm text-navy-800">
            Ce lien de réinitialisation est invalide ou a expiré. Merci de refaire une demande.
          </p>
          <Link
            to="/forgot-password"
            className="mt-6 inline-block text-sm font-medium text-brand-600 hover:text-brand-700 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-brand-500 focus-visible:ring-offset-1 rounded"
          >
            Demander un nouveau lien
          </Link>
        </div>
      </AuthLayout>
    );
  }

  return (
    <AuthLayout title="Réinitialiser le mot de passe" subtitle="Choisissez un nouveau mot de passe">
      <form onSubmit={handleSubmit(onSubmit)} noValidate>
        <PasswordInput
          label="Nouveau mot de passe"
          autoComplete="new-password"
          required
          hint="8 caractères min., avec majuscule, minuscule, chiffre et caractère spécial"
          error={errors.password?.message}
          {...register('password')}
        />

        <PasswordInput
          label="Confirmer le mot de passe"
          autoComplete="new-password"
          required
          error={errors.confirmPassword?.message}
          {...register('confirmPassword')}
        />

        <FormError
          message={
            resetPassword.isError
              ? 'Une erreur est survenue. Le lien est peut-être expiré.'
              : undefined
          }
        />

        <button
          type="submit"
          disabled={resetPassword.isPending}
          className="mt-2 w-full rounded-md bg-brand-600 px-4 py-2 text-sm font-semibold text-white shadow-sm transition-colors hover:bg-brand-700 focus:outline-none focus-visible:ring-2 focus-visible:ring-brand-500 focus-visible:ring-offset-2 disabled:opacity-60"
        >
          {resetPassword.isPending ? 'Validation...' : 'Réinitialiser le mot de passe'}
        </button>
      </form>
    </AuthLayout>
  );
}
