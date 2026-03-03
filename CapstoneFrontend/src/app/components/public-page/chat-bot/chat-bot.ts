import { Component, signal, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTooltipModule } from 'ng-zorro-antd/tooltip';
import { ChatService, ChatResponse } from '../../../services/chat/chat.service';

interface Message {
    text: string;
    type: 'user' | 'bot';
    audioBase64?: string;
    imageBase64?: string;
}

@Component({
    selector: 'app-chat-bot',
    standalone: true,
    imports: [CommonModule, FormsModule, NzIconModule, NzTooltipModule],
    templateUrl: './chat-bot.html',
    styleUrl: './chat-bot.css'
})
export class ChatBot implements AfterViewChecked {
    @ViewChild('scrollContainer') private scrollContainer!: ElementRef;

    isOpen = signal(false);
    isLoading = signal(false);
    audioEnabled = signal(false);
    userInput = '';
    messages = signal<Message[]>([
        { text: 'Hello! I am your Hartford Insurance Assistant. How can I help you today?', type: 'bot' }
    ]);

    constructor(private chatService: ChatService) { }

    ngAfterViewChecked() {
        this.scrollToBottom();
    }

    toggleChat() {
        this.isOpen.update(v => !v);
    }

    toggleAudio() {
        this.audioEnabled.update(v => !v);
    }

    sendMessage() {
        if (!this.userInput.trim() || this.isLoading()) return;

        const userMessage = this.userInput.trim();
        this.messages.update(msgs => [...msgs, { text: userMessage, type: 'user' as const }]);
        this.userInput = '';
        this.isLoading.set(true);

        this.chatService.askBot(userMessage, this.audioEnabled()).subscribe({
            next: (res: ChatResponse) => {
                this.isLoading.set(false);

                const botMessage: Message = {
                    text: res.response || "I'm sorry, I didn't get a proper response.",
                    type: 'bot',
                    audioBase64: res.audioBase64 || undefined,
                    imageBase64: res.imageBase64 || undefined
                };

                this.messages.update(msgs => [...msgs, botMessage]);

                // Auto-play audio if available
                if (botMessage.audioBase64) {
                    this.playAudio(botMessage.audioBase64);
                }

                setTimeout(() => this.scrollToBottom(), 100);
            },
            error: (err: any) => {
                console.error('[ChatBot] Error:', err);
                this.isLoading.set(false);
                this.messages.update(msgs => [...msgs, {
                    text: "Sorry, I'm having trouble connecting to the server. Please try again later.",
                    type: 'bot' as const
                }]);
                setTimeout(() => this.scrollToBottom(), 100);
            }
        });
    }

    playAudio(base64Data: string) {
        try {
            // The TTS API returns raw PCM data (s16le, 24000Hz, mono)
            const binaryString = atob(base64Data);
            const bytes = new Uint8Array(binaryString.length);
            for (let i = 0; i < binaryString.length; i++) {
                bytes[i] = binaryString.charCodeAt(i);
            }

            // Create WAV header for PCM data
            const wavBuffer = this.createWavBuffer(bytes, 24000, 1, 16);
            const blob = new Blob([wavBuffer], { type: 'audio/wav' });
            const url = URL.createObjectURL(blob);
            const audio = new Audio(url);
            audio.play().catch(err => console.error('Audio playback error:', err));
            audio.onended = () => URL.revokeObjectURL(url);
        } catch (err) {
            console.error('Audio play error:', err);
        }
    }

    private createWavBuffer(pcmData: Uint8Array, sampleRate: number, channels: number, bitsPerSample: number): ArrayBuffer {
        const dataLength = pcmData.length;
        const buffer = new ArrayBuffer(44 + dataLength);
        const view = new DataView(buffer);

        // RIFF header
        this.writeString(view, 0, 'RIFF');
        view.setUint32(4, 36 + dataLength, true);
        this.writeString(view, 8, 'WAVE');

        // fmt sub-chunk
        this.writeString(view, 12, 'fmt ');
        view.setUint32(16, 16, true); // sub-chunk size
        view.setUint16(20, 1, true); // PCM format
        view.setUint16(22, channels, true);
        view.setUint32(24, sampleRate, true);
        view.setUint32(28, sampleRate * channels * bitsPerSample / 8, true);
        view.setUint16(32, channels * bitsPerSample / 8, true);
        view.setUint16(34, bitsPerSample, true);

        // data sub-chunk
        this.writeString(view, 36, 'data');
        view.setUint32(40, dataLength, true);

        // Write PCM data
        const output = new Uint8Array(buffer, 44);
        output.set(pcmData);

        return buffer;
    }

    private writeString(view: DataView, offset: number, str: string) {
        for (let i = 0; i < str.length; i++) {
            view.setUint8(offset + i, str.charCodeAt(i));
        }
    }

    private scrollToBottom(): void {
        try {
            if (this.scrollContainer) {
                this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
            }
        } catch (err) { }
    }
}
