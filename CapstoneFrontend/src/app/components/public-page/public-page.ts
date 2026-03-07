import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import {
  LucideAngularModule,
  UserPlus,
  FileSearch,
  ClipboardCheck,
  ShieldCheck,
  Heart,
  AlertTriangle,
  Activity,
  Clock
} from 'lucide-angular';
import { ChatBot } from './chat-bot/chat-bot';
import { InsuranceTour } from './insurance-tour/insurance-tour';

@Component({
  selector: 'app-public-page',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    LucideAngularModule,
    ChatBot,
    InsuranceTour
  ],
  providers: [
    {
      provide: 'LUCIDE_ICONS',
      useValue: {  FileSearch, Clock, ClipboardCheck, AlertTriangle, ShieldCheck, Heart, Activity }
    }
  ],
  templateUrl: './public-page.html',
  styleUrl: './public-page.css',
})
export class PublicPage {
}
