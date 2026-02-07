import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AiAssistantService } from './ai-assistant.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Inject } from '@angular/core';

@Component({
  standalone: false,
  selector: 'app-ai-assistant',
  // imports: [],
  templateUrl: './ai-assistant.component.html',
  styleUrl: './ai-assistant.component.scss',
})
export class AiAssistantComponent {
  // @Input() employer: any;
  @Output() close = new EventEmitter<void>(); // ✅ THIS WAS MISSING

  messages: {
    role: 'user' | 'ai';
    text: string;
    time: Date;
  }[] = [];

  userQuestion = '';

  constructor(
    private aiService: AiAssistantService,
    @Inject(MAT_DIALOG_DATA) public employer: any,
    public dialogRef: MatDialogRef<AiAssistantComponent>
  ) { }

  ngOnChanges() {
    if (this.employer) {
      this.aiService.askEmployerInsight(this.employer, 'Give a quick insight and next steps').subscribe(
        res => {
          this.messages.push({
            role: 'ai',
            text: res.answer,
            time: new Date()
          });
        });
    }
  }

  askAI() {
    if (!this.userQuestion.trim()) return;

    const question = this.userQuestion;

    this.messages.push({
      role: 'user',
      text: question,
      time: new Date()
    });

    this.userQuestion = '';

    this.aiService.askEmployerInsight(this.employer, question)
      .subscribe({
        next: res => {
          this.messages.push({
            role: 'ai',
            text: res.answer,
            time: new Date()
          });

        },
        error: () => {
          this.messages.push({
            role: 'ai',
            text: '⚠️ Failed to get AI response.',
            time: new Date()
          });
        }
      });

    setTimeout(() => {
      const el = document.querySelector('.chat-window');
      el?.scrollTo({ top: el.scrollHeight, behavior: 'smooth' });
    });

  }

}
