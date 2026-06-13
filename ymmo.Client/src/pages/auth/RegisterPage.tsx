import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { Link } from 'react-router-dom';
import { AuthLayout } from '../../components/auth/AuthLayout';
import { FormError } from '../../components/form/FormError';
import { InputField } from '../../components/form/InputField';
import { PasswordInput } from '../../components/form/PasswordInput';
import { useRegister } from '../../hooks/useAuth';
import { registerSchema, type RegisterFormValues } from '../../schemas/authSchemas';

export default function RegisterPage() {
  const registerAccount = useRegister();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<RegisterFormValues>({
    resolver: zodResolver(registerSchema),
    mode: 'onBlur',
  });

  function onSubmit(values: RegisterFormValues) {
    registerAccount.mutate({
      firstName: values.firstName,
      lastName: values.lastName,
      email: values.email,
      password: values.password,
      phone: values.phone,
    });
  }

  return (
    <AuthLayout title="Créer un compte" subtitle="Rejoignez la plateforme Ymmo">
      <form onSubmit={handleSubmit(onSubmit)} noValidate>
        <div className="grid grid-cols-1 gap-x-4 sm:grid-cols-2">
          <InputField
            label="Prénom"
            autoComplete="given-name"
            required
            error={errors.firstName?.message}
            {...register('firstName')}
          />

          <InputField
            label="Nom"
            autoComplete="family-name"
            required
            error={errors.lastName?.message}
            {...register('lastName')}
          />
        </div>

        <InputField
          label="Email"
          type="email"
          autoComplete="email"
          required
          error={errors.email?.message}
          {...register('email')}
        />

        <InputField
          label="Téléphone"
          type="tel"
          autoComplete="tel"
          required
          error={errors.phone?.message}
          {...register('phone')}
        />

        <PasswordInput
          label="Mot de passe"
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
            registerAccount.isError
              ? 'Une erreur est survenue lors de la création du compte. Veuillez réessayer.'
              : undefined
          }
        />

        <button
          type="submit"
          disabled={registerAccount.isPending}
          className="mt-2 w-full rounded-md bg-brand-600 px-4 py-2 text-sm font-semibold text-white shadow-sm transition-colors hover:bg-brand-700 focus:outline-none focus-visible:ring-2 focus-visible:ring-brand-500 focus-visible:ring-offset-2 disabled:opacity-60"
        >
          {registerAccount.isPending ? 'Création...' : 'Créer mon compte'}
        </button>
      </form>

      <p className="mt-6 text-center text-sm text-navy-400">
        Vous avez déjà un compte ?{' '}
        <Link
          to="/login"
          className="font-medium text-brand-600 hover:text-brand-700 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-brand-500 focus-visible:ring-offset-1 rounded"
        >
          Se connecter
        </Link>
      </p>
    </AuthLayout>
  );
}
