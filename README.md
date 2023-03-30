# Chronicler AI 

ChroniclerAI is a desktop application that records, transcribes, and summarizes audio using the OpenAI GPT-3.5-turbo model. The application is built in C# with XAML for the user interface.

Turn any audio into query-able text: your really long technical meeting, that interview you couldn't make, or zoom training you were supposed to watch before end of week.

Quick download: Here is the zip file of the release build [here](https://github.com/KernAlan/ChroniclerAI/blob/master/ChroniclerAI/bin/Release/ChroniclerAI.zip)

![image](https://user-images.githubusercontent.com/63753020/228726833-d0102938-4be6-4ed5-85ce-0b7201c98549.png)

# Features

- Record live audio
- Transcribe recorded audio
- Summarize and save transcriptions
- Highlight key quotes from transcriptions
- Enumerate main points of transcriptions
- Ask questions or give commands related to transcriptions

# Prerequisites

Windows 10 or later
.NET 5 or later

# Installation

Quick download: Here is the zip file of the release build [here](https://github.com/KernAlan/ChroniclerAI/blob/master/ChroniclerAI/bin/Release/ChroniclerAI.zip)

Or, if you're a developer:

1. Clone the repo: git clone https://github.com/KernAlan/ChroniclerAI.git
2. Open the solution in Visual Studio.
3. Build the solution and run the application.

# Usage

1. (Optional) You can record live audio from your system input. This will be saved as recordedaudio.mp3 in your root folder of Chronciler. 
2. Enter your API key from Open AI -- or optionally press the "I need an API key" button to be directed to the page you can get one
3. Transcribe the recorded audio using the built-in transcription button. NOTE: This sends an API request to OpenAI's Whisper API using the API key you provided. Pricing is available on the OpenAI pricing page.
4. Choose one of the following actions for the transcribed text if you'd like to start dissecting your transcript:

- Summarize: Summarize the transcript with as much detail as possible.
- Highlight: Highlight key quotes from the transcript and provide a brief description of their importance.
- Enumerate: List the main points of the text in bullet points.
- Ask: Ask a question or give a command related to the transcript and receive an AI-generated response.

NOTE: This sends a Completion API request to OpenAI's API. Pricing is available on OpenAI's pricing page.

# Configuration

All you need is an API key from OpenAI. There is a button which links you directly to the page. Pricing is listed on the OpenAI pricing page -- let's face it, it's a steal.

# Contributing

Please feel free to contribute by submitting pull requests or opening issues to improve the application.

# License

This project is licensed under the MIT License. See the LICENSE file for details.
