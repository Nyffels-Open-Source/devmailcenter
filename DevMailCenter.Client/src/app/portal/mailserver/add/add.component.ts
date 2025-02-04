import {Component, OnInit} from '@angular/core';
import {ConfigClient, MailServerClient} from '../../../core/openapi/generated/openapi-client';
import {Subject, takeUntil} from 'rxjs';
import {CardModule} from 'primeng/card';
import {CommonModule, Location} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {ButtonModule} from 'primeng/button';
import {RippleModule} from 'primeng/ripple';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'dmc-mailserver-add',
  imports: [CardModule, CommonModule, FormsModule, ButtonModule, RippleModule],
  templateUrl: './add.component.html',
  styleUrl: './add.component.scss'
})
export class AddComponent implements OnInit {
  private destroy$ = new Subject<void>();

  providersLoaded = false;
  providers: string[] = [];

  providerCards: { provider: string, name?: string, logoUrl?: string }[] = [];

  selectedProvider: string | null = null;

  constructor(private configClient: ConfigClient, private mailServerClient: MailServerClient, private location: Location, private router: Router, private activetedRoute: ActivatedRoute) {}

  ngOnInit() {
    this.configClient.listEnableProviders()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: providers => {
          this.providers = providers;
          this.generateProviderCards();
          this.providersLoaded = true;
        },
        error: error => {
          // TODO Error handling toast. Do not show add!
        }
      })
  }

  generateProviderCards() {
    this.providerCards = this.providers.map(provider => {
      return {
        provider: provider,
        name: {'Smtp': 'Basic SMTP', 'MicrosoftExchange': 'Microsoft Exchange 365 / Outlook.com', 'Google': 'Gmail'}[provider] ?? 'Unknown provider',
        logoUrl: {'Smtp': 'providers/smtp.png', 'MicrosoftExchange': 'providers/exchange.png', 'Google': 'providers/google.png'}[provider] ?? "",
      }
    })
  }

  async onSelectProvider(provider: string) {
    this.selectedProvider = provider;

    switch (provider) {
      case 'Smtp': {
        this.router.navigate(['./smtp'], {relativeTo: this.activetedRoute});
        break;
      }
      case 'MicrosoftExchange': {
        Object.keys(sessionStorage)
          .filter(k => /^msal\..*/.test(k))
          .reduce(function (obj: any, key: string) {
            sessionStorage.removeItem(key);
          }, {});

        this.mailServerClient.getMicrosoftAuthenticationUrl(window.location.origin + "/callback/microsoft")
          .pipe(takeUntil(this.destroy$))
          .subscribe({next: url => {window.open(url, "_self")}});
        break;
      }
      case 'Google': {
        alert("Not yet implemented!")
        break;
      }
    }
  }

  onCancel() {
    this.location.back();
  }
}
