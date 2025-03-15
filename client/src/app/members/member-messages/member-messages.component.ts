import { Component, inject, input, OnInit, output } from '@angular/core';
import { MessageService } from '../../_services/message.service';
import { Message } from '../../_models/message';
import { FormsModule } from '@angular/forms';
import { TimeagoModule } from 'ngx-timeago';

@Component({
  selector: 'app-member-messages',
  standalone: true,
  imports: [TimeagoModule, FormsModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css'
})
export class MemberMessagesComponent implements OnInit {
  private messageService = inject(MessageService);
  username = input.required<string>();
  messages = input.required<Message[]>();
  messageContent = '';
  updateMessages = output<Message>();

  ngOnInit(): void {
    
  }
}
