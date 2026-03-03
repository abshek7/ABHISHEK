import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { Claim } from '../../../services/claim/claim';

@Component({
  selector: 'app-customer-claim-status',
  standalone: true,
  imports: [CommonModule, NzTableModule, NzTagModule],
  templateUrl: './customer-claim-status.html',
  styleUrl: './customer-claim-status.css',
})
export class CustomerClaimStatus implements OnInit {
  private claimService = inject(Claim);
  claims = this.claimService.myClaims;

  ngOnInit() {
    this.claimService.loadMyClaims();
  }
}

