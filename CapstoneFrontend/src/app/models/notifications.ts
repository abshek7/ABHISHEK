export type NotificationType = 'PolicyRenewal' | 'PaymentDue' | 'ClaimUpdate';

export interface Notification {
  type: NotificationType;
  message: string;
  createdAt: string;
}

