import api from './api';
import type { LoginPayload, TokenResponse, UserProfile } from '../types';

export const authService = {
  login: async (payload: LoginPayload): Promise<TokenResponse> => {
    const { data } = await api.post<TokenResponse>('/api/auth/login', payload);
    return data;
  },

  me: async (): Promise<UserProfile> => {
    const { data } = await api.get<UserProfile>('/api/auth/me');
    return data;
  },

  logout: async (refreshToken: string): Promise<void> => {
    await api.post('/api/auth/logout', { refreshToken });
  },
};
